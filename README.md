# rideaway-backend(v 0.3.0)

Backend for rideaway: a bicycle navigation application

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
### Get a geoJSON with all the routes of the GFR network

Launch a `GET` request to `hostname/routes/GFR.json`
