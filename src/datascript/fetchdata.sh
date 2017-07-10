#!/bin/bash

# Download the latest belgium osm.pbf file from geofabrik
wget http://download.geofabrik.de/europe/belgium-latest.osm.pbf -P ../mapdata --backups=1

# Run IDP to convert the osm.pbf to a routerdb
./idp/IDP --read-pbf ../mapdata/belgium-latest.osm.pbf --pr --create-routerdb vehicles=../profiles/bicycle.lua --write-routerdb ../mapdata/belgium.routerdb
