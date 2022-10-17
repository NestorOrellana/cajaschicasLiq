$(function () {


    $('#autorizadores').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 500,
        heigth: 250,
        title: 'Edicion Orden Nro:',
        open: function (event, ui) {

            //initialize();
            //loadOrder($(this).data('orderId'));

        },
        close: function (event, ui) {

            //limpia todos los textbox del popup

        },
        buttons: {
            Cerrar: function () {
                $(this).dialog("close");
            }
        }
    });


    $('#btnAutorizadores').click(function () {
        $("#autorizadores").dialog({
            title: "Autorizador",

            resizable: false,
            height: 200,
            width: 350,
            modal: true,
            open: function (event, ui) {
                BuscarAprobador();
            },
            buttons: {
                "Cerrar": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#autorizadores').dialog('open');
        return false;
    });

    $('#popuporderedit').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 500,
        heigth: 250,
        title: 'Edicion Orden Nro:',
        open: function (event, ui) {

            //initialize();
            //loadOrder($(this).data('orderId'));

        },
        close: function (event, ui) {

            //limpia todos los textbox del popup
            $('#popuporderedit :text').val('');

        },
        buttons: {
            Actualizar: function () {

                alert('funciona');

            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialog-confirm').dialog({
        autoOpen: false,
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });



    $("#AlmacenarFactura").click(function () {


        $("#dialog-confirm").dialog({
            title: "Almacenar factura",

            resizable: false,
            height: 200,
            width: 350,
            modal: true,
            buttons: {
                "Aceptar": function () {
                    //__doPostBack(uniqueID, '');
                    OcultarMensaje();
                    AlmacenarFactura();
                    $(this).dialog("close");
                    $('#txtNumeroFactura').focus();

                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#dialog-confirm').dialog('open');
        $('#txtNumeroFactura').focus();
        return false;
    });




    function AlmacenarFactura() {
        var facturaEncabezadoDto = new Object();
        var mensaje;
        facturaEncabezadoDto = ConstruirObjetoJason();

        mensaje = ""

        mensaje = ValidarDatos(facturaEncabezadoDto);
        if (mensaje != "") {
            DesplegarError(mensaje);
            return;
        }

        var params = new Object();
        params.facturaEncabezadoDto = facturaEncabezadoDto;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../../RegistroFacturas/MapeoUsuariosCentros.asmx/AlmacenarFactura",
            data: JSON.stringify(params),
            dataType: "json",
            async: true,
            success: function (response) {
                var ret = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;

                if (ret.CODIGO == 0) {

                    InicializarIdFactura();
                    DesplegarError(ret.MENSAJE);
                    alert(ret.MENSAJE);
                    $('#txtNumeroFactura').focus();
                }
                else {
                    NuevaFactura();
                    DesplegarMensaje(ret.MENSAJE);
                    $('#txtNumeroFactura').focus();
                }
            },
            error: function (response, status, error) {
                DesplegarError("error al almacenar la factura.");
            }
        });

        //        var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/AlmacenarFactura';
        //        $.ajax({
        //            type: "POST",
        //            contentType: "application/json; charset=utf-8",
        //            url: url,
        //            data: JSON.stringify(params),
        //            dataType: "json",
        //            async: true,
        //            success: function (response) {
        //                var ret = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;

        //                if (ret.CODIGO == 0) {

        //                    InicializarIdFactura();
        //                    DesplegarError(ret.MENSAJE);
        //                    alert(ret.MENSAJE);
        //                }
        //                else {
        //                    NuevaFactura();
        //                    DesplegarMensaje(ret.MENSAJE);
        //                }
        //            },
        //            error: function (response, status, error) {
        //                DesplegarError("error al almacenar la factura." + error);
        //            }
        //        });
    }

    function ValidarDatos(facturaEncabezado) {
        var mensaje = "";

        if ((facturaEncabezado.CENTRO_COSTO == "-1" && facturaEncabezado.ORDEN_COSTO == "1-") || (facturaEncabezado.CENTRO_COSTO == "" && facturaEncabezado.ORDEN_COSTO == "")) {
            mensaje += "Debe seleccionar un centro de costo o una orden de costo. <br />";
        }

        if (facturaEncabezado.ID_PROVEEDOR == 0) {
            mensaje += "Debe seleccionar un proveedor. <br />";
        }

        if (facturaEncabezado.NUMERO_IDENTIFICACION == "") {
            mensaje += "El n&uacute;mero de identificaci&oacute;n no puede estar en blanco. <br />";
        }

        if (isNaN(facturaEncabezado.NUMERO) || (facturaEncabezado.NUMERO == "")) {
            mensaje += "Debe escribir el n&uacute;mero de la factura.<br />";
        }


        if (facturaEncabezado.FECHA_FACTURA == "") {
            mensaje += "Debe escribir la fecha de la factura.<br />";
        }

        if (facturaEncabezado.RETENCION_IVA == true) {
            if ((facturaEncabezado.NUMERO_RETENCION_IVA == "") || (facturaEncabezado.VALOR_RETENCION_IVA == "")) {
                mensaje += "Debe escribir el n&uacute;mero de retenci&oacute;n y el valor de la retenci&oacute;n del IVA.<br />";
            }

            if ((facturaEncabezado.NUMERO_RETENCION_IVA == 0) || (facturaEncabezado.VALOR_RETENCION_IVA == 0)) {
                mensaje += "El n&uacute;mero de retenci&oacute;n y el valor de la retenci&oacute;n del IVA deben ser mayores a 0.<br />";
            }

            if (facturaEncabezado.NUMERO_RETENCION_IVA == null) {
                mensaje += "Debe escribir el n&uacute;mero de retenci&oacute;n.<br />";
            }
        }

        if (facturaEncabezado.RETENCION_ISR == true) {
            if ((facturaEncabezado.NUMERO_RETENCION_ISR == "") || (facturaEncabezado.VALOR_RETENCION_ISR == "")) {
                mensaje += "Debe escribir el n&uacute;mero de retenci&oacute;n y el valor de la retenci&oacute;n del ISR.<br />";
            }

            if ((facturaEncabezado.NUMERO_RETENCION_ISR == 0) || (facturaEncabezado.VALOR_RETENCION_ISR == 0)) {
                mensaje += "El n&uacute;mero de retenci&oacute;n y el valor de la retenci&oacute;n del ISR deben ser mayores a 0.<br />";
            }

            if (facturaEncabezado.NUMERO_RETENCION_ISR == null) {
                mensaje += "Debe escribir el n&uacute;mero de retenci&oacute;n.<br />";
            }
        }

        if (facturaEncabezado.PAIS == "SV") {
            if ((facturaEncabezado.SERIE == null) || (facturaEncabezado.SERIE == "")) {
                mensaje += "Debe escribir el n&uacute;mero de Serie.<br />";
            } else {
                    if (facturaEncabezado.SERIE.length > 8)
                        mensaje += "La Serie debe contener menos de 9 caracteres<br />"
                    if (facturaEncabezado.NUMERO.length > 8)
                        mensaje += "El n&uacute;mero de factura debe contener menos de 9 caracteres<br />"
            }

        }

        var numero = 0;
        var sumaIVA = 0;
        var sumaTotal = 0;

        $.each(facturaEncabezado.FACTURA_DETALLE, function (i) {
            numero++;

            if (parseFloat(facturaEncabezado.FACTURA_DETALLE[i].IMPUESTO) >= parseFloat(facturaEncabezado.FACTURA_DETALLE[i].VALOR)) {
                mensaje += ("El impuesto es igual o mayor al valor de compra en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
            };

            if ((facturaEncabezado.FACTURA_DETALLE[i].DESCRIPCION).length > 50) {
                mensaje += ("El largo de la descripción es mayor a 50 caracteres en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
            };

            sumaTotal = parseFloat(sumaTotal) + parseFloat(facturaEncabezado.FACTURA_DETALLE[i].VALOR);
            sumaIVA = parseFloat(sumaIVA) + parseFloat(facturaEncabezado.FACTURA_DETALLE[i].IVA);


            $.each(facturaEncabezado.FACTURA_DETALLE[i], function (key, val) {

                if (key == "DESCRIPCION") {
                    if ($.trim(val) == "") {
                        mensaje += ("Debe de escribir descripci&oacute;n en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }
                }

                if (key == "CANTIDAD") {
                    if ($.trim(val) == "") {
                        mensaje += ("Debe de escribir una cantidad en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }

                    if (val == 0) {
                        mensaje += ("La cantidad debe ser mayor a 0 en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }

                    if (isNaN(val)) {
                        mensaje += ("La cantidad tiene valor invalido en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }
                }

                if (key == "VALOR") {
                    if ($.trim(val) == "") {
                        mensaje += ("Debe de escribir el valor en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }

                    if (val == 0) {
                        mensaje += ("El valor debe ser mayor a 0 en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }

                    if (isNaN(val)) {
                        mensaje += ("El valor tiene valor invalido en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }
                }

                if (key == "IMPUESTO") {
                    if (isNaN(val)) {
                        mensaje += ("El impuesto tiene valor invalido en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }
                }

                if (key == "CUENTA_CONTABLE") {
                    if ($.trim(val) == "::Cuenta contable::") {
                        mensaje += ("Debe de seleccionar una cuenta contable en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }

                    if ($.trim(val) == "") {
                        mensaje += ("Debe de seleccionar una cuenta contable en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }

                    if ($.trim(val) == "-1") {
                        mensaje += ("Debe de seleccionar una cuenta contable en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }
                }

                if (key == "IDENTIFICADOR_IVA") {
                    if ($.trim(val) == "::Tipo IVA::") {
                        mensaje += ("Debe de seleccionar tipo IVA en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }


                    if ($.trim(val) == "") {
                        mensaje += ("Debe de seleccionar tipo IVA en el detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                    }
                }

//                if (facturaEncabezado.CODIGO_SOCIEDAD == "1440" || facturaEncabezado.CODIGO_SOCIEDAD == "1630") {
                if (facturaEncabezado.PAIS == "CR"){
                    if (key == "DETALLE") {
                        if ($.trim(val) == "") {
                            mensaje += ("Debe de ingresar detalle en la l&iacute;nea n&uacute;mero " + numero + ".<br />");
                        }
                    }
                }

            });

        });

        if (parseFloat(facturaEncabezado.IVA) != parseFloat(sumaIVA.toFixed(2).toString())) {
            mensaje += ("La sumatoria del IVA de la factura es diferente al IVA del encabezado.<br />");
        }

        if (parseFloat(facturaEncabezado.VALOR_TOTAL) != parseFloat(sumaTotal)) {
            mensaje += ("La sumatoria del valor del detalle de la factura es diferente al valor total del encabezado.<br />");
        }

        return mensaje;
    }
});

