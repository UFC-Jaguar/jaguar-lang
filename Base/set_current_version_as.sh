#!/bin/bash
old_version="$1"
new_version="$2"
# Remove all dots
old_no_dots="${old_version//./}"
new_no_dots="${new_version//./}"
old_version="net$old_no_dots"
new_version="net$new_no_dots"
#echo "old_no_dots: $old_no_dots"
#echo "new_no_dots: $new_no_dots"
#echo "old_version: $old_version"
#echo "new_version: $new_version"
version="$new_version"
#echo "Default new version: $version"
if [ -z "$1" ]
then
	echo "######################### INCLUDE OLD VERSION  #########################"
	echo "Set the version as: "
	echo "  ./set_current_version_as.sh old_version new_version"
	echo "Ex:"
	echo "  ./set_current_version_as.sh 4.8 4.5"
else
  if [ -z "$2" ]
  then
	echo "######################### INCLUDE NEW VERSION  #########################"
	echo "Set the version as: "
	echo "  ./set_current_version_as.sh old_version new_version"
	echo "Ex:"
	echo "  ./set_current_version_as.sh 4.8 4.5"
  else
	echo "cp Jaguar.sln Jaguar.$version.sln"
	cp Jaguar.sln Jaguar.$version.sln

	folder="Jaguar"
	echo "cp $folder/$folder.csproj $folder/$folder.$version.csproj"
	cp $folder/$folder.csproj $folder/$folder.$version.csproj

	echo "Running: ./scripts/rename_words_in_file_csproj.sh $1 $2"
	./scripts/rename_words_in_file_csproj.sh $1 $2

	echo "Running: ./scripts/rename_words_in_file_sln.sh $version"
	./scripts/rename_words_in_file_sln.sh $version

	echo "********************************** OK! To compile, USE: **********************************"
	echo "xbuild NPB-CSharp.$version.sln"
	echo "******************************************************************************************"
  fi
fi

