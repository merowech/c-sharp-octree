.DEFAULT_GOAL=build

build:
	mcs -out:test.exe *.cs octree/*.cs
	mono test.exe
