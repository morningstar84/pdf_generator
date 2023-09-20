#!/bin/sh
# convert.sh

set -e

file="$1"
cmd="unoconv --connection 'socket,host=localhost,port=8100;urp;StarOffice.ComponentContext' -f pdf /pdfs/${file}"

echo "Starting PDF conversion -> ${file}"
echo "[CMD]: Executing -> ${cmd}"
eval ${cmd}
echo "[CMD]: Execution completed -> ${cmd}"