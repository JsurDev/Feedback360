//const contenedorBotones = document.getElementById('contenedor-botones');
const contenedorToast = document.getElementById('contenedor-toast');

// Event listener para detectar click en los botones
//contenedorBotones.addEventListener('click', (e) => {
//	e.preventDefault();

//	const tipo = e.target.dataset.tipo;

//	if (tipo === 'exito') {
//		agregarToast({ tipo: 'exito', titulo: 'Exito!', descripcion: 'La operación fue exitosa.', autoCierre: true });
//	}
//	if (tipo === 'error') {
//		agregarToast({ tipo: 'error', titulo: 'Error', descripcion: 'Hubo un error', autoCierre: true });
//	}
//	if (tipo === 'info') {
//		agregarToast({ tipo: 'info', titulo: 'Info', descripcion: 'Esta es una notificación de información.' });
//	}
//	if (tipo === 'warning') {
//		agregarToast({ tipo: 'warning', titulo: 'Warning', descripcion: 'Ten cuidado' });
//	}
//});

// Event listener para detectar click en los toasts
contenedorToast.addEventListener('click', (e) => {
	const toastId = e.target.closest('div.toast').id;

	if (e.target.closest('button.btn-cerrar')) {
		cerrarToast(toastId);
	}
});

// Función para cerrar el toast
const cerrarToast = (id) => {
	document.getElementById(id)?.classList.add('cerrando');
};

// Función para agregar la clase de cerrando al toast.
const agregarToast = ({ tipo, titulo, descripcion, autoCierre }) => {
	// Crear el nuevo toast
	const nuevoToast = document.createElement('div');

	// Agregar clases correspondientes
	nuevoToast.classList.add('toast');
	nuevoToast.classList.add(tipo);
	if (autoCierre) nuevoToast.classList.add('autoCierre');

	// Agregar id del toast
	const numeroAlAzar = Math.floor(Math.random() * 100);
	const fecha = Date.now();
	const toastId = fecha + numeroAlAzar;
	nuevoToast.id = toastId;

	// Iconos
	const iconos = {
		info: `<i class="bi bi-bell-fill"></i>`,

	};

	// Plantilla del toast
	const toast = `<div class="contenido">
				${iconos[tipo]}
                <div class="icono">
                    <i class="bi bi-bell-fill"></i>
                </div>
                <div class="texto">
                    <p class="titulo">${titulo}</p>
                    <p class="descripcion">${descripcion}</p>
                </div>
            </div>
            <button class="btn-cerrar">
                <div class="icono">
                    <i class="bi bi-x"></i>
                </div>
            </button>`;

	// Agregar la plantilla al nuevo toast
	nuevoToast.innerHTML = toast;

	// Agregamos el nuevo toast al contenedor
	contenedorToast.appendChild(nuevoToast);

	// Función para menajera el cierre del toast
	const handleAnimacionCierre = (e) => {
		if (e.animationName === 'cierre') {
			nuevoToast.removeEventListener('animationend', handleAnimacionCierre);
			nuevoToast.remove();
		}
	};

	if (autoCierre) {
		setTimeout(() => cerrarToast(toastId), 5000);
	}

	// Agregamos event listener para detectar cuando termine la animación
	nuevoToast.addEventListener('animationend', handleAnimacionCierre);
};
