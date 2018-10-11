@set rootPath=..

..\ConfigGen\bin\Debug\ConfigGen.exe -optMode all ^
-configXml %rootPath%\Csv\Cfg.xml ^
-dataDir %rootPath%\Assets\Config ^
-xmlCodeDir %rootPath%\Assets\XmlCode

@pause