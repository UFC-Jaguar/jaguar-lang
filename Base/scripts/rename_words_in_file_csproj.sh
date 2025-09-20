#!/bin/bash
old_version="$1"
new_version="$2"
# Remove all dots
old_no_dots="${old_version//./}"
new_no_dots="${new_version//./}"
old_version="net$old_no_dots"
new_version="net$new_no_dots"

if [ -z "$1" ]
then
        echo "##################################################################"
        echo "Digitar './scripts/rename_words_in_file_csproj.sh old_version new_version'"
        echo "Example: './scripts/rename_words_in_file_csproj.sh 4.8 4.5'"
        echo "##################################################################"
else
    if [ -z "$2" ]
	then
        echo "##################################################################"
        echo "Digitar './scripts/rename_words_in_file_csproj.sh old_version new_version'"
        echo "Example: './scripts/rename_words_in_file_csproj.sh 4.8 4.5'"
        echo "##################################################################"
	else
		OLD="v$1"
		NEW="v$2"
		echo "OLD: $OLD , NEW: $NEW"
		find Jaguar/Jaguar.$new_version.csproj | xargs sed -i 's/'"$OLD"'/'"$NEW"'/g'
	fi
fi



