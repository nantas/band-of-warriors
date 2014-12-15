#!/bin/bash
export DEST="./.exvim.app"
export TOOLS="/Users/nantas/.vim/tools/"
export TMP="${DEST}/_symbols"
export TARGET="${DEST}/symbols"
sh ${TOOLS}/shell/bash/update-symbols.sh
