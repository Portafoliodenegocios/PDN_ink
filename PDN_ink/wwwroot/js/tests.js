// Pruebas Unitarias Simples para DashboardApp

const TestSuite = {
    run() {
        console.log('%c Iniciando Pruebas Unitarias... ', 'background: #222; color: #bada55; font-size: 14px; padding: 4px;');
        
        this.testUtils();
        this.testNavigation();
        this.testFilters();
        this.testFolders();
        
        console.log('%c Pruebas Finalizadas. ', 'background: #222; color: #bada55; font-size: 14px; padding: 4px;');
    },

    assert(condition, message) {
        if (condition) {
            console.log(`%c✅ PASS: ${message}`, 'color: green;');
        } else {
            console.error(`❌ FAIL: ${message}`);
        }
    },

    testUtils() {
        console.group('Utils Tests');
        
        // Test generateRandomString
        if (DashboardApp.Utils && DashboardApp.Utils.generateRandomString) {
            const str = DashboardApp.Utils.generateRandomString(10);
            this.assert(str.length === 10, 'generateRandomString debe retornar longitud correcta');
            this.assert(typeof str === 'string', 'generateRandomString debe retornar un string');
        } else {
            console.warn('⚠️ No se pudo acceder a DashboardApp.Utils.generateRandomString');
        }

        // Test parseDate
        if (DashboardApp.Utils && DashboardApp.Utils.parseDate) {
            const date1 = DashboardApp.Utils.parseDate('2023-12-25');
            this.assert(date1 instanceof Date && !isNaN(date1), 'parseDate debe manejar formato YYYY-MM-DD');
            this.assert(date1.getDate() === 25 && date1.getMonth() === 11, 'parseDate debe parsear correctamente la fecha');

            const date2 = DashboardApp.Utils.parseDate('25/12/2023');
            this.assert(date2 instanceof Date && !isNaN(date2), 'parseDate debe manejar formato DD/MM/YYYY');
        } else {
            console.warn('⚠️ No se pudo acceder a DashboardApp.Utils.parseDate');
        }

        console.groupEnd();
    },

    testNavigation() {
        console.group('Navigation Tests');
        if (DashboardApp.Navigation) {
            this.assert(typeof DashboardApp.Navigation.loadPage === 'function', 'Navigation.loadPage debe ser una función');
            // No podemos probar la navegación real fácilmente sin un entorno de navegador completo con fetch mockeado,
            // pero verificamos que el módulo exista.
        } else {
            console.error('❌ DashboardApp.Navigation no está definido');
        }
        console.groupEnd();
    },

    testFilters() {
        console.group('Filters Tests');
        if (DashboardApp.Filters) {
            this.assert(typeof DashboardApp.Filters.init === 'function', 'Filters.init debe ser una función');
            this.assert(typeof DashboardApp.Filters.runFilters === 'function', 'Filters.runFilters debe ser una función');
        } else {
            console.error('❌ DashboardApp.Filters no está definido');
        }
        console.groupEnd();
    },

    testFolders() {
        console.group('Folders Tests');
        if (DashboardApp.Folders) {
            this.assert(typeof DashboardApp.Folders.createFolder === 'function', 'Folders.createFolder debe ser una función');
            // Podríamos intentar simular la creación de una carpeta si tuviéramos acceso al estado interno o DOM
        } else {
            console.error('❌ DashboardApp.Folders no está definido');
        }
        console.groupEnd();
    }
};

// Ejecutar pruebas después de un breve retraso para asegurar que main.js haya cargado
setTimeout(() => {
    if (typeof DashboardApp !== 'undefined') {
        TestSuite.run();
    } else {
        console.error('❌ DashboardApp no está definido. Asegúrate de cargar main.js antes de tests.js');
    }
}, 1000);