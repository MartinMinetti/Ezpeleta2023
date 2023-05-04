window.onload = BuscarServicios();



function BuscarServicios() 
{
    $("#tbody-servicios").empty();
    $.ajax({
        url : '../../Servicios/BuscarServicios',

        data : { },

        type : 'GET',

        dataType : 'json',

        success : function(servicios)
        {
            $("#tbody-servicios").empty();
            $.each(servicios, function( index, servicio){

                let claseEliminada = 'table-success';
                let botones = '<buttom type="button" class="btn btn-warning btn-sm" onClick="BuscarServicio(' + servicio.servicioID + ')">Editar </buttom> '+
                '<buttom type="button" class="btn btn-danger btn-sm" onClick="EliminarServicio(' + servicio.servicioID +')">Deshabilitar</buttom> ';

                if (servicio.eliminado){
                    claseEliminada  = 'table-danger';
                    botones = '<buttom type="button" class="btn btn-success btn-sm" onClick="EliminarServicio(' + servicio.servicioID + ')">Activar</buttom>';
                }

                $("#tbody-servicios").append('<tr class=' + claseEliminada + '>' +
                '<td>' + servicio.descripcion + '</td>' +
                '<td>' + servicio.direccion + '</td>' +
                '<td>' + servicio.telefono + '</td>' +
                '<td>' + servicio.subcategoriaNombre  + '</td>' +
                '<td>' + servicio.categoriaNombre  + '</td>' +
                '<td class="text-center">' +
                botones +
                '</td>' +
                '</tr>');
            });
        },

        error : function(xhr, status) {
            alert('Error en el listado');
        },
    })
}




function VaciarFormulario(){
    $("#ServicioID").val(0);

    $("#ServicioNombre").val('');
    $("#Error-ServicioNombre").text("");

    $("#DirecciónServicio").val('');
    $("#Error-DirecciónServicio").text("");

    $("#TelefonoServicio").val('');
    $("#Error-TelefonoServicio").text("");
}




function GuardarServicio(){

    //JAVASCRIPT
    $("#Titulo-Modal-Servicio").text("Nuevo servicio");
    $("#Error-ServicioNombre").text("");
    let servicioID = $("#ServicioID").val();
    let subcategoriaID = $("#SubcategoriaID").val();
    let servicioNombre =  $("#ServicioNombre").val().trim();
    let direccionServicio = $("#DirecciónServicio").val();
    let telefonoServicio = $("#TelefonoServicio").val();

    if(servicioNombre != "" && servicioNombre != null)
    {
        $.ajax({

            url : '../../Servicios/GuardarServicio',

            data : { ServicioID: servicioID , Descripcion: servicioNombre, SubcategoriaID: subcategoriaID, Direccion: direccionServicio, Telefono: telefonoServicio },

            type : 'POST',

            dataType : 'json',

            success : function(resultado) {
                if (resultado == 0) {

                    $("#Modal-servicio").modal("hide");
                    BuscarServicios();
                }
                if (resultado == 2){
                    $("#Error-ServicioNombre").text("El servicio ingresado ya existe.");
                }
            },

            error : function(xhr, status) {
                alert('Disculpe existio un problema.');
            },
        });
    }
    else
    {
        $("#Error-ServicioNombre").text("Debe ingresar un nombre.");
        $("#Error-DirecciónServicio").text("Debe ingresar una dirección.");
        $("#Error-TelefonoServicio").text("Debe ingresar un telefono.");
    }
}


function BuscarServicio(servicioID){
    $("#Titulo-Modal-Servicio").text("Editar servicio");
    $("#ServicioID").val(servicioID);

    $.ajax({
        url : '../../Servicios/BuscarServicios',

        data : { ServicioID: servicioID },

        type : 'GET',

        dataType : 'json',


        success : function(servicios) {
        
            console.log(servicios);
        if (servicios.length == 1){
            let servicio = servicios [0];
            console.log(servicios);
            $("#ServicioID").val(servicio.servicioID);
            $("#ServicioNombre").val(servicio.descripcion);
            $("#DirecciónServicio").val(servicio.direccion);
            $("#TelefonoServicio").val(servicio.telefono);
            $("#SubCategoriaID").val(servicio.subcategoriaID);
            $("#Modal-servicio").modal("show");
        
        }


        },


        error : function(xhr, status) {
            alert('Error al cargar servicios.');
        },


        complete : function(xhr, status) {

        }
    });
}



function EliminarServicio(servicioID, habilitar){
    $.ajax({
        url : '../../Servicios/EliminarServicio',

        data : { ServicioID: servicioID, Habilitar: habilitar },

        type : 'POST',

        dataType : 'json',

        success : function(resultado) {
            if (resultado) {
                BuscarServicios();
            }
            else{
                alert("No se puede eliminar.");
            }
        },

        error : function(xhr, status) {
            alert('Disculpe existio un problema.');
        },
    });
}
