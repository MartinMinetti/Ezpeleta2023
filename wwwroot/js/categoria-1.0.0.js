window.onload = BuscarCategorias();


function BuscarCategorias(){

    $("#tbody-categorias").empty();
    $.ajax({
        // la URL para la petición
        url : '../../Categorias/BuscarCategorias',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(categorias) {

            $("#tbody-categorias").empty();
            $.each(categorias, function( index, categoria){

                    let claseEliminado = 'table-success';
                    let botones = '<buttom type="button"  style="margin: 10px" class="btn btn-warning btn-sm" onClick="BuscarCategoria(' + categoria.categoriaID + ')">Editar </buttom> '+
                    '<buttom type="button" style="margin: 10px" class="btn btn-danger btn-sm" onClick="EliminarCategoria(' + categoria.categoriaID +')">Deshabilitar</buttom> ';

                    if (categoria.eliminada){
                        claseEliminado  = 'table-danger';
                        botones = '<buttom type="button"  style="margin: 10px" class="btn btn-success btn-sm" onClick="EliminarCategoria(' + categoria.categoriaID + ')">Activar</buttom>';
                    }
                    
                    $("#tbody-categorias").append('<tr class=' + claseEliminado + '>' +
                    '<td>' + categoria.descripcion + '</td>' +
                    '<td class="d-flex justify-content-end">' + botones + '</td>' +
                    '</tr>');
            }
            )
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error : function(xhr, status) {
            alert('Error al cargar categorias');
        },
   
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            //alert('Petición realizada');
        }

    });
}

function VaciarFormulario(){
    $("#Descripcion").val('');
    $("#CategoriaID").val(0);
}

function BuscarCategoria(categoriaID){

    $.ajax({
        // la URL para la petición
        url : '../../Categorias/BuscarCategorias',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { categoriaID: categoriaID },
    
        // especifica si será una petición POST o GET
        type : 'GET',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(categorias) {
            
            if (categorias.length == 1){
                let categoria = categorias[0];
                $("#Descripcion").val(categoria.descripcion);
                $("#CategoriaID").val(categoria.categoriaID);

                $("#ModalCategoria").modal("show");
            }
            

        },
    
        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error : function(xhr, status) {
            alert('Error al cargar categorias');
        },
    
        // código a ejecutar sin importar si la petición falló o no
        complete : function(xhr, status) {
            // alert('Petición realizada');
        }
    });
}


function GuardarCategoria(){

    //JAVASCRIPT
    let descripcion1 = document.getElementById("Descripcion").value;
    let descripcion2 = $("#Descripcion").val();
    let categoriaID = $("#CategoriaID").val();

    $.ajax({
        // la URL para la petición
        url : '../../Categorias/GuardarCategoria',
    
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data : { categoriaID : categoriaID , descripcion : descripcion1 },
    
        // especifica si será una petición POST o GET
        type : 'POST',
    
        // el tipo de información que se espera de respuesta
        dataType : 'json',
    
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success : function(resultado) {
            if (resultado) {

                $("#ModalCategoria").modal("hide");
                BuscarCategorias();
            }
            else{
                alert("Existe una categoría con la misma descripción.");
            }
        },
    
        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error : function(xhr, status) {
            alert('Disculpe existio un problema.');
        },
    });
}



function EliminarCategoria(categoriaID, elimina){

    $.ajax({
       
        url : '../../Categorias/EliminarCategoria',
    
        data : { CategoriaID: categoriaID , Elimina: elimina },
    
        type : 'POST',
    
        dataType : 'json',

        success : function(resultado) {
            if (resultado == 0) {
                BuscarCategorias();
            }
            else{
                alert("No se puede eliminar porque contiene subcategorias activos.");
            }
        },
    
        error : function(xhr, status) {
            alert('Disculpe existio un problema.');
        },
    });
}

