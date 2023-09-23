# PIT - Permissions Inheritance Tree

PIT is a simple command line tool that help to discover broken permission inheritance on files and directories. It traverses the all the subdirectories showing the status of permission inheritance of files and directories.

Examples: 

Problems on folder 2023\H1 and on file 2023\H2\foo.txt:

```
> PIT.exe C:\Temp\MyData

Processing: C:\Temp\MyData

 - MyData
     - 2022
         - H1
            - bar.txt
            - foo.txt
         - H2
            - bar.txt
            - foo.txt
     - 2023
         X H1
            - bar.txt
            - foo.txt
         - H2
            - bar.txt
            X foo.txt
```


Same problems, in a more concise view:

```
> PIT.exe C:\Temp\MyData  --brokenOnly

Processing: C:\Temp\MyData

X C:\Temp\MyData\2023\H1
X C:\Temp\MyData\2023\H2\foo.txt
```

