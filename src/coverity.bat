
set output=c:\tmp\coverity\objectprinter
cov-build --dir %output%\cov-int msbuild /t:Rebuild
7z a -r %output%.zip %output%
curl --form project=ObjectPrinter --form token=MowVRvISN77MeD3Lve66zQ --form email=drewburlingame@gmail.com --form file=@%output%.zip --form version=2.0.3 http://scan5.coverity.com/cgi-bin/upload.py