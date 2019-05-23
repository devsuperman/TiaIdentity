var $form = document.querySelector('form');

      $form.addEventListener('submit', (e) => {

        e.preventDefault();

        var url = $form.action;
        var obj = new FormData($form);
        var $botao = document.querySelector('[type=submit]');                        
        
        $form.reset();

        $botao.setAttribute('disabled', true);
        $botao.value = 'Email Enviado!';

        Swal.fire("Yes!", "Enviamos um email com um link para alterar sua senha!", "success");
        
        var xml = new XMLHttpRequest();
        xml.open('post', url);
        xml.send(obj);

      });