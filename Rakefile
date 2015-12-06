require 'bundler/setup'

require 'albacore'
require 'albacore/tasks/release'
require 'albacore/tasks/versionizer'
require 'albacore/ext/teamcity'

Configuration = ENV['CONFIGURATION'] || 'Release'
Description = 'An F# wrapper for GraphViz'

Albacore::Tasks::Versionizer.new :versioning

desc 'create assembly infos'
asmver_files :assembly_info do |a|
  a.files = FileList['src/**/*.fsproj'] # optional, will find all projects recursively by default

  a.attributes assembly_description: Description,
               assembly_configuration: Configuration,
               assembly_copyright: "(c) 2015 by github.com/misterspeedy",
               assembly_version: ENV['LONG_VERSION'],
               assembly_file_version: ENV['LONG_VERSION'],
               assembly_informational_version: ENV['BUILD_VERSION']

  a.handle_config do |proj, conf|
    conf.namespace = conf.namespace + "AsmVer"
    conf
  end
end

desc 'Perform fast build (warn: doesn\'t d/l deps)'
build :quick_compile do |b|
  b.prop 'Configuration', Configuration
  b.logging = 'detailed'
  b.sln     = 'src/FsDot.sln'
end

task :paket_bootstrap do
system 'tools/paket.bootstrapper.exe', clr_command: true unless   File.exists? 'tools/paket.exe'
end

desc 'restore all nugets as per the packages.config files'
task :restore => :paket_bootstrap do
  system 'tools/paket.exe', 'restore', clr_command: true
end

desc 'Perform full build'
build :compile => [:versioning, :restore, :assembly_info] do |b|
  b.prop 'Configuration', Configuration
  b.sln = 'src/FsDot.sln'
end

directory 'build/pkg'

desc 'package nugets - finds all projects and package them'
nugets_pack :create_nugets => ['build/pkg', :versioning, :compile] do |p|
  p.configuration = Configuration
  p.files   = FileList['src/**/*.fsproj'].exclude(/Archie|Tests/)
  p.out     = 'build/pkg'
  p.exe     = 'packages/NuGet.CommandLine/tools/NuGet.exe'
  p.with_metadata do |m|
    # m.id          = 'MyProj'
    m.title       = 'FsDot'
    m.description = Description
    m.authors     = 'misterspeedy'
    m.project_url = 'https://github.com/haf/fsdot'
    m.tags        = 'fsharp graphviz fsdot plotting'
    m.version     = ENV['NUGET_VERSION']
  end
end

namespace :tests do
  task :fsdot do
    system 'packages/NUnit.Runners/tools/nunit-console.exe',
           "src/FsDot.Tests/bin/#{Configuration}/FsDot.Tests.dll",
           clr_command: true
  end

  task :graph do
    system 'packages/NUnit.Runners/tools/nunit-console.exe',
           "src/Graph.Tests/bin/#{Configuration}/Graph.Tests.dll",
           clr_command: true
  end

  task :unit => %i|fsdot graph|
end

task :tests => :'tests:unit'

task :default => %i|create_nugets|

task :ensure_nuget_key do
  raise 'missing env NUGET_KEY value' unless ENV['NUGET_KEY']
end

Albacore::Tasks::Release.new :release,
                             pkg_dir: 'build/pkg',
                             depend_on: [:create_nugets, :ensure_nuget_key],
                             nuget_exe: 'packages/NuGet.CommandLine/tools/NuGet.exe',
                             api_key: ENV['NUGET_KEY']
