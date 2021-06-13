function ConfirmBorrar(Id, SeHizoClic) {
    var confirmborrar = 'BorrarSpan_' + Id;
    var confirmBorrarSpan = 'confirmBorrarSpan_' + Id;

    if (SeHizoClic) {
        $('#' + confirmborrar).hide();
        $('#' + confirmBorrarSpan).show();

    }
    else {
        $('#' + confirmborrar).show();
        $('#' + confirmBorrarSpan).hide();

    }

}