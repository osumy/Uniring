document.addEventListener('DOMContentLoaded', ()=>{

  document.querySelectorAll('.field').forEach(field=>{
    const input = field.querySelector('input');
    if(!input) return;
    if(input.value.trim() !== '') field.classList.add('filled');

    input.addEventListener('focus', ()=> field.classList.add('focused'));
    input.addEventListener('blur', ()=> field.classList.remove('focused'));
    input.addEventListener('input', ()=>{
      if(input.value.trim() !== '') field.classList.add('filled');
      else field.classList.remove('filled');
      if(input.name === 'password' || input.name === 'confirm_password'){
        checkPasswordMatch(field.closest('form'));
      }
      if(input.name === 'phone'){
        formatPhoneInput(input);
      }
    });
  });

  document.querySelectorAll('.toggle-pass').forEach(btn=>{
    btn.addEventListener('click', e=>{
      const field = e.currentTarget.closest('.field');
      const input = field.querySelector('input');
      if(input.type === 'password'){ input.type = 'text'; e.currentTarget.textContent = '🔓'; }
      else { input.type = 'password'; e.currentTarget.textContent = '🔒'; }
    });
  });

  document.querySelectorAll('form.auth').forEach(form=>{
    form.addEventListener('submit', (e)=>{
      e.preventDefault();
      clearErrors(form);
      const name = form.querySelector('[name="username"]')?.value.trim();
      const phone = form.querySelector('[name="phone"]')?.value.trim();
      const email = form.querySelector('[name="email"]')?.value.trim();
      const pass = form.querySelector('[name="password"]')?.value;
      const confirm = form.querySelector('[name="confirm_password"]')?.value;
      let ok = true;

      if(form.dataset.type === 'register'){
        if(!name || name.length < 2){ showError(form,'username','Enter a valid username'); ok=false; }
        if(!phone || !/^09\d{9}$/.test(phone)){ showError(form,'phone','Enter a valid mobile (e.g. 09123456789)'); ok=false; }
      }

      if(email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)){ showError(form,'email','Enter a valid email'); ok=false; }
      if(!pass || pass.length < 6){ showError(form,'password','Password must be at least 6 characters'); ok=false; }
      if(form.dataset.type === 'register'){
        if(pass !== confirm){ showError(form,'confirm_password','Passwords do not match'); ok=false; }
      }

      if(!ok) return;

      const btn = form.querySelector('button[type="submit"]');
      const original = btn.textContent;
      btn.disabled = true;
      btn.textContent = form.dataset.type === 'register' ? 'Registering...' : 'Signing in...';

      setTimeout(()=>{
        btn.disabled = false;
        btn.textContent = original;
        alert(form.dataset.type === 'register' ? 'Registered successfully' : 'Signed in successfully');
        form.reset();
        form.querySelectorAll('.field').forEach(f=> f.classList.remove('filled'));
      },900);

    });
  });

  function showError(form, fieldName, msg){
    const f = form.querySelector(`[name="${fieldName}"]`);
    if(!f) return;
    let p = f.parentElement.querySelector('.error');
    if(!p){ p = document.createElement('div'); p.className = 'error'; f.parentElement.appendChild(p); }
    p.textContent = msg; p.classList.add('show');
  }
  function clearErrors(form){
    form.querySelectorAll('.error').forEach(e=> e.remove());
  }

  function checkPasswordMatch(form){
    if(!form) return;
    const pass = form.querySelector('[name="password"]')?.value || '';
    const confirm = form.querySelector('[name="confirm_password"]')?.value || '';
    const node = form.querySelector('[name="confirm_password"]')?.parentElement;
    if(!node) return;
    let err = node.querySelector('.error');
    if(pass && confirm && pass !== confirm){
      if(!err){ err = document.createElement('div'); err.className='error'; node.appendChild(err); }
      err.textContent = 'Passwords do not match'; err.classList.add('show');
    } else {
      if(err) err.remove();
    }
  }

  function formatPhoneInput(input){
    input.value = input.value.replace(/[^\d]/g,'').slice(0,11);
  }

});
