(defun-q S::STARTUP ( )
	(command "_netload" "Z:\\Programy\\MAKRA\\geogeo\\Geo-geo.dll")
	(command "_cuiunload" "GEOGEO")
	(vla-load (vla-get-menugroups (vlax-get-acad-object)) "Z:\\Programy\\MAKRA\\geogeo\\geogeo.cuix")
    (princ))