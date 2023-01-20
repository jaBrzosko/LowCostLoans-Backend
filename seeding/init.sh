#!/bin/bash

docker cp seed.sql backend-database:seed.sql
docker exec -it backend-database psql -U admin -d backend -a -f seed.sql
