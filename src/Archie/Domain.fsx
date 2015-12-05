#load "Domain.fs"

open Archie

module TicketStore =

    let insertTicket = Activity("Insert Ticket" , Command, Sql)
    let retrieveTicket = Activity("Get Ticket" , Query, Sql)
    let updateLastLive = Activity("Update Last Live" , Command, Sql)
    let updateStatus = Activity("Update Status" , Command, Sql)
    let getForStatus = Activity("Get For Status" , Query, Sql)

    let ticketStore =
        Component("Ticket Store", Store, Singleton)
            .Provides(insertTicket)
            .Provides(retrieveTicket)
            .Provides(updateLastLive)
            .Provides(updateStatus)
            .Provides(getForStatus)

module ResourceManager =

    let getTicket = Activity("Get Ticket" , CommandQuery, Http)
    let getTicketStatus = Activity("Get Ticket Status" , Query, Http)
    let notifyStillAlive = Activity("Notify Still Alive" , Command, Http)
    let completeTicket = Activity("Complete Ticket" , Command, Http)

    let resourceManager =
        Component("Resource Manager", Processor, Singleton)
            .Provides(getTicket)
            .Provides(getTicketStatus)  
            .Provides(notifyStillAlive)
            .Provides(completeTicket)
            .Uses(TicketStore.insertTicket)
            .Uses(TicketStore.retrieveTicket)
            .Uses(TicketStore.updateLastLive)
            .Uses(TicketStore.updateStatus)

module Client = 
    let client = 
        Component("Client", Client, Multiple)
            .Uses(ResourceManager.getTicket)
            .Uses(ResourceManager.getTicketStatus)  
            .Uses(ResourceManager.completeTicket)

module ResourceController =

    let requestScale = Activity("Request Scale", Command, AzureServiceBus)

    let resourceController =
        Component("Resource Controller", Processor, Singleton)
            .Provides(requestScale)

module Scheduler =

    let notifyResourceAvailable = Activity("Notify Available", Command, Http)

    let scheduler =
        Component("Scheduler", Processor, Singleton)
            .Uses(TicketStore.getForStatus)
            .Uses(ResourceController.requestScale)
