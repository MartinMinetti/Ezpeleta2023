// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//abrir el menu responsivo

let menu = document.querySelector('#menu-icon');
let nabvar2 = document.querySelector('.nabvar2');

menu.onclick = () => {
	menu.classList.toggle('bx-x');
	nabvar2.classList.toggle('open');
}