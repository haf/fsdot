module Archie.ResourceManager

open Archie

module TicketStore =

    let insertTicket = Activity.New("Insert Ticket" , Command, Sql)
    let retrieveTicket = Activity.New("Get Ticket" , Query, Sql)
    let updateLastLive = Activity.New("Update Last Live" , Command, Sql)
    let updateStatus = Activity.New("Update Status" , Command, Sql)
    let getForStatus = Activity.New("Get For Status" , Query, Sql)
    // Omitted GetForResourceType and GetForResourceTypeAndStatus

module ResourceManager =

    let getTicket = Activity.New("Get Ticket" , CommandQuery, Http)
    let getAllocationStatus = Activity.New("Get Allocation Status" , Query, Http)
    let notifyStillAlive = Activity.New("Notify Still Alive" , Command, Http)
    let notifyComplete = Activity.New("Notify Complete" , Command, Http)

module ResourceController =

    let requestScaleUpTo = Activity.New("Request Scale Up To", Command, AzureServiceBus)
    let requestScaleDown = Activity.New("Request Scale Down", Command, AzureServiceBus)

module DynamoDbScalingRequestQueue =
    
    let addRequest = Activity.New("Add Request", Command, InProcess)
    let getNextWaiting = Activity.New("Get Next Waiting Requests", Query, InProcess)
    let setUpdating = Activity.New("Set To Updating", Command, InProcess)
    let getUpdating = Activity.New("Get Updating Requests", Query, InProcess)
    let setCompleted = Activity.New("Set To Completed", Command, InProcess)
    let getCompleted = Activity.New("Get Completed Requests", Query, InProcess)
    let removeRequest = Activity.New("Remove Request", Command, InProcess)

module AWSDyanamoDBApi =

    let describeTable = Activity.New("Describe Table", Query, Http)
    let updateTable = Activity.New("Update Table", Command, Http)

module DynamoDBProxy =
    
    let getTableStatus = Activity.New("Get Table Status", Query, InProcess)
    let scaleTableReadsTo = Activity.New("Scale Table Reads To", Command, InProcess)
    let getCurrentProvisioning = Activity.New("Get Current Provisioning", Query, InProcess)

module DynamoDbScaler =

    let serveWaitingRequests = Activity.New("Serve Waiting Requests", Command, InProcess)
    let serveUpdatingRequests = Activity.New("Serve Updating Requests", CommandQuery, InProcess)

module Scheduler =

    let notifyAllocationCompleted = Activity.New("Notify Allocation Completed", Command, AzureServiceBus)

module Components = 

    let ticketStore =
        Component("Ticket Store", Store, Singleton)
            .Provides(TicketStore.insertTicket)
            .Provides(TicketStore.retrieveTicket)
            .Provides(TicketStore.updateLastLive)
            .Provides(TicketStore.updateStatus)
            .Provides(TicketStore.getForStatus)

    let resourceManager =
        Component("Resource Manager", Processor, Singleton)
            .Provides(ResourceManager.getTicket)
            .Provides(ResourceManager.getAllocationStatus)  
            .Provides(ResourceManager.notifyStillAlive)
            .Provides(ResourceManager.notifyComplete)
            .Uses(TicketStore.updateLastLive)
            .Uses(TicketStore.retrieveTicket)
            .Uses(TicketStore.insertTicket)

    let resourceController =
        Component("Resource Controller", Processor, Singleton)
            .Provides(ResourceController.requestScaleUpTo)
            .Provides(ResourceController.requestScaleDown)
            .Uses(Scheduler.notifyAllocationCompleted)
            .Uses(DynamoDbScalingRequestQueue.addRequest)
            .Uses(DynamoDbScaler.serveUpdatingRequests)
            .Uses(DynamoDbScaler.serveWaitingRequests)

    let client = 
        Component("Client", Client, Multiple)
            .Uses(ResourceManager.notifyComplete)
            .Uses(ResourceManager.notifyStillAlive)  
            .Uses(ResourceManager.getAllocationStatus)  
            .Uses(ResourceManager.getTicket)

    let dynamoDbScalingRequestQueue = 
        Component("DynamoDB Scaling Request Queue", Processor, Singleton)
            .Provides(DynamoDbScalingRequestQueue.addRequest)
            .Provides(DynamoDbScalingRequestQueue.getNextWaiting)
            .Provides(DynamoDbScalingRequestQueue.setUpdating)
            .Provides(DynamoDbScalingRequestQueue.getUpdating)
            .Provides(DynamoDbScalingRequestQueue.setCompleted)
            .Provides(DynamoDbScalingRequestQueue.getCompleted)
            .Provides(DynamoDbScalingRequestQueue.removeRequest)

    let awsDynamoDBApi =
        Component("AWS DynamoDB API", Processor, Singleton)
            .Provides(AWSDyanamoDBApi.describeTable)
            .Provides(AWSDyanamoDBApi.updateTable)

    let dynamoDbProxy =
        Component("DynamoDB Proxy", Processor, Singleton)
            .Provides(DynamoDBProxy.getTableStatus)
            .Provides(DynamoDBProxy.scaleTableReadsTo)
            .Provides(DynamoDBProxy.getCurrentProvisioning)
            .Uses(AWSDyanamoDBApi.updateTable)
            .Uses(AWSDyanamoDBApi.describeTable)

    let dynamoDbScaler =
        Component("DynamoDB Scaler", Processor, Singleton)
            .Provides(DynamoDbScaler.serveWaitingRequests)
            .Provides(DynamoDbScaler.serveUpdatingRequests)
            .Uses(DynamoDBProxy.getTableStatus)
            .Uses(DynamoDBProxy.scaleTableReadsTo)
            .Uses(DynamoDbScalingRequestQueue.setCompleted)
            .Uses(DynamoDbScalingRequestQueue.getUpdating)
            .Uses(DynamoDbScalingRequestQueue.setUpdating)
            .Uses(DynamoDbScalingRequestQueue.getNextWaiting)

    let scheduler =
        Component("Scheduler", Processor, Singleton)
            .Provides(Scheduler.notifyAllocationCompleted)
            .Uses(ResourceController.requestScaleUpTo)
            .Uses(ResourceController.requestScaleDown)
            .Uses(DynamoDBProxy.getCurrentProvisioning)
            .Uses(TicketStore.updateStatus)
            .Uses(TicketStore.getForStatus)

open Components

let resourceManager = 
    {
        Name = "Resource Manager Architecture"
        Components = 
            [
                ticketStore
                resourceManager
                client
                resourceController
                dynamoDbScalingRequestQueue
                awsDynamoDBApi
                dynamoDbProxy
                dynamoDbScaler
                scheduler
            ]
    }
