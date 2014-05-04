
set outputroot=c:\tmp\coverity\objectprinter
set output=%outputroot%\cov-int
set zip=%outputroot%.zip

rm -fr %output%
cov-build --dir %output% msbuild /t:Rebuild /p:Configuration=Release
7z a -r %zip% %output%
curl --form project=ObjectPrinter --form token=MowVRvISN77MeD3Lve66zQ --form email=drewburlingame@gmail.com --form file=@%zip% --form version=2.0.3 http://scan5.coverity.com/cgi-bin/upload.py