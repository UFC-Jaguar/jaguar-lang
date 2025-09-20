#!/bin/bash
version="$1"
if [ -z "$version" ]
then
        echo "##################################################################"
        echo "Digitar './scripts/rename_words_in_file_sln.sh version_name'"
        echo "Example: './scripts/rename_words_in_file_sln.sh net45'"
        echo "##################################################################"
else
	OLD1="\.csproj"
	NEW1="\.$version\.csproj"
	find . -iname *.$version.sln | xargs sed -i 's/'"$OLD1"'/'"$NEW1"'/g'
fi

