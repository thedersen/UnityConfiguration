require 'rubygems'
require 'albacore'
require 'rake/clean'

VERSION = "1.2.0.0"
OUTPUT = "build"
CONFIGURATION = 'Release'
ASSEMBLY_INFO = 'src/UnityConfiguration/Properties/AssemblyInfo.cs'
SOLUTION_FILE = 'src/UnityConfiguration.sln'

Albacore.configure do |config|
	config.log_level = :verbose
	config.msbuild.use :net4
end

desc "Compiles solution and runs unit tests"
task :default => [:clean, :version, :compile, :test, :publish, :package]

desc "Executes all NUnit tests"
task :test => [:nunit]

#Add the folders that should be cleaned as part of the clean task
CLEAN.include(OUTPUT)
CLEAN.include(FileList["src/**/#{CONFIGURATION}"])
CLEAN.include("TestResult.xml")

desc "Update assemblyinfo file for the build"
assemblyinfo :version => [:clean] do |asm|
	asm.version = VERSION
	asm.company_name = "UnityConfiguration"
	asm.product_name = "UnityConfiguration"
	asm.title = "UnityConfiguration"
	asm.description = "Convention based configuration API for the Microsoft Unity IoC container"
	asm.copyright = "Copyright (C) Thomas Pedersen"
	asm.output_file = ASSEMBLY_INFO
end

desc "Compile solution file"
msbuild :compile => [:version] do |msb|
	msb.properties :configuration => CONFIGURATION
	msb.targets :Clean, :Build
	msb.solution = SOLUTION_FILE
end

desc "Gathers output files and copies them to the output folder"
task :publish => [:compile] do
	Dir.mkdir(OUTPUT)
	Dir.mkdir("#{OUTPUT}/binaries")

	FileUtils.cp_r FileList["src/**/#{CONFIGURATION}/*.dll"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries"
end

desc "Executes NUnit tests"
nunit :nunit => [:compile] do |nunit|
	tests = FileList["src/**/#{CONFIGURATION}/*.Tests.dll"].exclude(/obj\//)

    nunit.command = "src/packages/NUnit.2.5.7.10213/Tools/nunit-console-x86.exe"
	nunit.assemblies = tests
end	

desc "Creates a NuGet packaged based on the UnityConfiguration.nuspec file"
exec :package => [:publish] do |cmd|
	Dir.mkdir("#{OUTPUT}/nuget")
	cmd.command = "tools/nuget.exe"
	cmd.parameters "pack UnityConfiguration.nuspec -o #{OUTPUT}/nuget"
end