# geo-geo
---
Rozszerzenie dla geodetów i nie tylko dla Autocad 2018+

---

Najważniejsze funkcje przedstawione na gifie:

---
Instalacja
Aby podpiąć do Autocada trzeba zmodyfikować plik star_up.lsp:

(defun-q S::STARTUP ( )
; wskazać sieżkę do bin/debug/Geo-geo.dll
	(command "_netload" "Z:\\Programy\\MAKRA\\geogeo\\Geo-geo.dll")
	(command "_cuiunload" "GEOGEO")
; wskazać sieżkę do geogeo.cuix
	(vla-load (vla-get-menugroups (vlax-get-acad-object)) "Z:\\Programy\\MAKRA\\geogeo\\geogeo.cuix")
    (princ))

a następnie poleceniem _appload wskazać zmodyfikowany plik lsp do Pakietów uruchomieniowych przy starcie programu, restart autocada i już powino działać.

![acad1](https://github.com/user-attachments/assets/7e0b85e7-7c3f-45fb-b27d-459288a8b238)

Czasami może sie pojawić problem z bezpieczeństwem dlatego można zmienić ustawienie _Secureload z 1 na 0.
Jeśli Autocad niby ładuje wszystko, ale nadal ma probelmy mżna zmodyfikować plik C:\Program Files\Autodesk\AutoCAD {wersja cada}\acad.exe.config na ten wzór:

<configuration>

  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0"/>
  </startup>

  <runtime>
    <AppContextSwitchOverrides value="Switch.System.ServiceModel.DisableUsingServicePointManagerSecurityProtocols=false;Switch.System.Net.DontEnableSchUseStrongCrypto=false" />
  </runtime>

<!--All assemblies in AutoCAD are fully trusted so there's no point generating publisher evidence-->
   <runtime>        
	<generatePublisherEvidence enabled="false"/>    
   </runtime>

  <appSettings>
    <add key="EnableWindowsFormsHighDpiAutoResizing" value="true" />
  </appSettings>
</configuration>
