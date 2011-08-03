DataSift API
============

This is the official C# library for accessing DataSift. See the example
projects for some simple example usage. 

The unit test project (or any of the demos) should run by setting it as the start up project for the solution.


Requirements
------------
* Microsoft .NET 3.5
* Note that you need a compiler capable of compiling .NET 3.5 code
* (Possibly) An IDE such as visual studio...the express version is free and should do fine. (Note that you cannot* debug unit test with the express version)

The following libraries are included in the lib folder.

* JSON.NET from http://json.codeplex.com/ (Included)

Get going
---------
The entire repository is on Visual studio solution.
There are 4 projects in the solution
* DataSift - This is the Datasift library. You can get the files in the bin/[debug|release] folder and add a reference in your project to get started or get the source and compile
* DatasiftTest - This project contains a set of unit tests for the Datasift library
* DatasiftApiDemo - This project demonstrates interacting with api.datasift.net
* DatasiftStreamDemo - This project demonstrates interacting with stream.datasift.net


License
-------

All code contained in this repository is Copyright 2011 MediaSift Ltd.

This code is released under the BSD license. Please see the LICENSE file for more details.
