
var $form = document.querySelector('form');

$form.addEventListener('submit', function (e) {
    e.preventDefault();

    var obj = new FormData(this);
    var url = this.action;
    var $botao = $form.querySelector('[type=submit]');                        
    
    this.reset();

    $botao.setAttribute('disabled', true);
    $botao.innerHTML = '<i class="fa fa-check-circle"></i> Email Enviado!';
    
    Swal("Sucesso!", "Enviamos um email com um link para alterar sua senha!", "success");                
    
    var xhr = new XMLHttpRequest();
    xhr.open('POST', url, true);
    xhr.send(obj);
});
