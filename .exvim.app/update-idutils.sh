#!/bin/bash
export DEST="./.exvim.app"
export TOOLS="/Users/nantas/.vim/tools/"
export TMP="${DEST}/_ID"
export TARGET="${DEST}/ID"
sh ${TOOLS}/shell/bash/update-idutils.sh
