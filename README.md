# rideaway-backend(v 1.3.3)

Backend for rideaway: a bicycle navigation application. Provides a routing endpoint where you can provide two coordinates to get the route between these points. Different routing profiles can be used to get different routing behavior and there is support for route instructions.

## Installation

Make sure you have the .NET Core library installed https://www.microsoft.com/net/core
Run `build.sh` or `build.bat` to build it and `run.sh` or `run.bat` to run it. The Api will start on http://localhost:5000

## Api

### Get a route

Launch a `GET` request to `hostname/route`

#### Parameters

- `loc1` and `loc2`: The starting and ending coordinate of the route (example: `loc1=50.86071,4.35614`)
- `profile`: choose a profile to do routing, possible values are:
	- `networks`: use the bicycle networks as much as possible
	- `balanced`: tries to select calmer streets
	- `shortest`: the shortest route, but maybe not the most pleasant one.
	- `brussels`: use the brussels bicycle network as much as possible (not complete)
- `instructions`: Boolean to specify if you want the API to return route instructions or not.
- `lang`: specify the language of the instructions (supported: `en` and `nl`).

### Get a geoJSON with all the routes of the GFR network

Launch a `GET` request to `hostname/routes/GFR.json`
