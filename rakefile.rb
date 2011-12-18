require 'rubygems'
require 'albacore'
require 'rake/clean'

VERSION = "1.4.1"
DESCRIPTION = "Convention based configuration API for the Microsoft Unity IoC container. With just a few lines of code, you can now registere all your classes in the entire solution. If the built-in conventions doesn't fit your needs, it is very easy to extend with your own."

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
CLEAN.include("*.nuspec")

desc "Update assemblyinfo file for the build"
assemblyinfo :version => [:clean] do |asm|
	asm.version = VERSION
	asm.company_name = "UnityConfiguration"
	asm.product_name = "UnityConfiguration"
	asm.title = "UnityConfiguration"
	asm.description = DESCRIPTION
	asm.copyright = "Copyright (C) 2011 Thomas Pedersen"
	asm.output_file = ASSEMBLY_INFO
	asm.com_visible = false
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

	FileUtils.cp_r FileList["src/**/#{CONFIGURATION}/*.*"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries"
end

desc "Executes NUnit tests"
nunit :nunit => [:compile] do |nunit|
	tests = FileList["src/**/#{CONFIGURATION}/*.Tests.dll"].exclude(/obj\//)

    nunit.command = "src/packages/NUnit.2.5.10.11092/Tools/nunit-console-x86.exe"
	nunit.assemblies = tests
end

desc "Creates a NuGet packaged based on the UnityConfiguration.nuspec file"
nugetpack :package => [:publish, :nuspec] do |nuget|
	Dir.mkdir("#{OUTPUT}/nuget")
	
    nuget.command = "src/.nuget/nuget.exe"
    nuget.nuspec = "UnityConfiguration.nuspec"
	nuget.base_folder = "#{OUTPUT}/binaries/"
    nuget.output = "#{OUTPUT}/nuget/"
	nuget.symbols = true
end

desc "Create the nuget package specification"
nuspec do |nuspec|
    nuspec.id ="UnityConfiguration"
    nuspec.version = VERSION
    nuspec.authors = "Thomas Pedersen (thedersen)"
    nuspec.description = DESCRIPTION
    nuspec.language = "en-US"
    nuspec.licenseUrl = "http://thedersen.mit-license.org/"
    nuspec.projectUrl = "https://github.com/thedersen/UnityConfiguration"
	nuspec.tags = "unity ioc convention auto"
	nuspec.file "UnityConfiguration.dll", "lib/net35"
	nuspec.file "UnityConfiguration.pdb", "lib/net35"
	nuspec.file "UnityConfiguration.xml", "lib/net35"
	nuspec.file "..\\..\\src\\**\\*.cs", "src"
    nuspec.dependency "Unity", "2.0"
    nuspec.output_file = "UnityConfiguration.nuspec"
end

desc "Pushes and publishes the NuGet package to nuget.org"
nugetpush :release => [:package] do |nuget|
    nuget.command = "src/.nuget/nuget.exe"
    nuget.package = "#{OUTPUT}/nuget/UnityConfiguration.#{VERSION}.nupkg"
    nuget.create_only = false
end