# KLDSampleApp
Write a command line application (preferably in C#) that does the following:   
1.	Takes two inputs and a flag
 - A directory that contains the files to be analyzed
 - A path for the output file (including file name and extension)
 - A flag to determine whether or not to include subdirectories contained (and all subsequently embedded subdirectories) within the input directory ([a.] above)
2.	Processes each of the files in the directory (and subdirectories if the flag is present)
3.	Determines using a file signature if a given file is a PDF or a JPG
 - JPG files start with 0xFFD8
 - PDF files start with 0x25504446
4.	For each file that is a PDF or a JPG, creates an entry in the output CSV containing the following information 
 - The full path to the file
 - The actual file type (PDF or JPG)
 - The MD5 hash of the file contents
