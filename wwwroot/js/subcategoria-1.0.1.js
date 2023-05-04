window.onload = BuscarSubcategorias();



function BuscarSubcategorias()
{
    $("#tbody-subcategorias").empty();
    $.ajax({
        url : '../../Subcategorias/BuscarSubcategorias',

        data : { },

        type : 'GET',

        dataType : 'json',
    
        success : function(subcategorias) 
        {
            $("#tbody-subcategorias").empty();

            $.each(subcategorias, function( index, subcategoria){
                
                let claseEliminado = 'table-success';
                let botones = '<buttom type="button"  class="btn btn-warning btn-sm" onClick="BuscarSubcategoria(' + subcategoria.subcategoriaID + ')">Editar </buttom> '+
                '<buttom type="button" class="btn btn-danger btn-sm" onClick="EliminarSubcategoria(' + subcategoria.subcategoriaID +')">Deshabilitar</buttom> ';

                if (subcategoria.eliminada){
                    claseEliminado  = 'table-danger';
                    botones = '<buttom type="button" class="btn btn-success btn-sm" onClick="EliminarSubcategoria(' + subcategoria.subcategoriaID + ')">Activar</buttom>';
                }
                
                $("#tbody-subcategorias").append('<tr class=' + claseEliminado + '>' +
                '<td>' + subcategoria.descripcion + '</td>' +
                '<td>' + subcategoria.categoriaNombre + '</td>' +
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
    $("#SubCategoriaNombre").val('');
    $("#SubcategoriaID").val(0);
    $("#Error-SubcategoriaNombre").text("");
}




function GuardarSubcategoria(){

    //JAVASCRIPT
    $("#Error-SubcategoriaNombre").text("");
    let subcategoriaID = $("#SubcategoriaID").val();
    let categoriaID = $("#CategoriaID").val();
    let subcategoriaNombre =  $("#SubCategoriaNombre").val().trim();

    if(subcategoriaNombre != "" && subcategoriaNombre != null)
    {
        $.ajax({

            url : '../../Subcategorias/GuardarSubcategoria',
        
            data : { SubcategoriaID: subcategoriaID , Descripcion: subcategoriaNombre, CategoriaID: categoriaID },
        
            type : 'POST',
        
            dataType : 'json',
        
            success : function(resultado) {
                if (resultado == 0) {
    
                    $("#Modal-subcategoria").modal("hide");
                    BuscarSubcategorias();
                }
                if (resultado == 2){
                    $("#Error-SubcategoriaNombre").text("La subcategoría ingresada ya existe.");
                }
            },
        
            error : function(xhr, status) {
                alert('Disculpe existio un problema.');
            },
        });
    }
    else
    {
        $("#Error-SubcategoriaNombre").text("Debe ingresar un nombre.");
    }
}


function BuscarSubcategoria(subCategoriaID){
    $("#Titulo-Modal-Subrucategoria").text("Editar subcategoría");
    $("#SubcategoriaID").val(subCategoriaID);
    $.ajax({
        url : '../../Subcategorias/BuscarSubcategorias',
    
        data : { SubcategoriaID: subCategoriaID },
    
        type : 'GET',
    
        dataType : 'json',
    

        success : function(subcategorias) {
        console.log(subcategorias);
        if(subcategorias.length == 1){
            let subcategoria = subcategorias[0];
            console.log(subcategoria);
            $("#SubCategoriaNombre").val(subcategoria.descripcion);
            $("#SubcategoriaID").val(subcategoria.subcategoriaID);
            $("#CategoriaID").val(subcategoria.categoriaID);
            $("#Modal-subcategoria").modal("show");
        }          
        
            
        },
    

        error : function(xhr, status) {
            alert('Error al cargar subcategorías');
        },
    
  
        complete : function(xhr, status) {

        }
    });
}



function EliminarSubcategoria(subcategoriaID, elimina){
    $.ajax({
        url : '../../Subcategorias/EliminarSubcategoria',

        data : { SubcategoriaID: subcategoriaID, Elimina: elimina },
       
        type : 'POST',
    
        dataType : 'json',
    
        success : function(resultado) {
            if (resultado == 0) {
                BuscarSubcategorias();
            }
            else{
                alert("No se puede eliminar porque contiene servicios activos.");
            }
        },
    
        error : function(xhr, status) {
            alert('Disculpe existio un problema.');
        },
    });
}




