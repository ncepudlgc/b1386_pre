#!/bin/bash
NAMESPACE="${1:-codebase_b1386_app}"
docker build -t "$NAMESPACE" .