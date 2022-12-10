#!/bin/bash

cd ..

if [ ! -d "API" ] ; then
  git clone git@ssh.dev.azure.com:v3/javadotnetproject/Loan/API API
fi

cd Backend

# Run docker compose for this backend
docker-compose -f src/docker-compose.yaml up -d --build

