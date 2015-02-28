#!/bin/bash
export DEST="./.exvim.app"
export TOOLS="/Users/nantas/.vim/tools/"
export IS_EXCLUDE=
export FOLDERS=""
export FILE_SUFFIXS="cs|cpp|json|html|js|css|c"
export TMP="${DEST}/_files"
export TARGET="${DEST}/files"
export ID_TARGET="${DEST}/idutils-files"
sh ${TOOLS}/shell/bash/update-filelist.sh
