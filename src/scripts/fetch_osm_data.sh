#!/bin/bash

# Download the latest belgium osm.pbf file from geofabrik.
# Downloaded files get placed into the mapdata folder.
wget http://download.geofabrik.de/europe/belgium-latest.osm.pbf -P ../mapdata --backups=1

# Run IDP to convert the osm.pbf to a routerdb.
# IDP is the data processing tool to convert a raw osm.pbf file to a
# format optimized for routing.
idp/rhel.7-x64/IDP --read-pbf ../mapdata/belgium-latest.osm.pbf --pr --create-routerdb vehicles=../profiles/bicycle.lua --write-routerdb ../mapdata/belgium.routerdb
