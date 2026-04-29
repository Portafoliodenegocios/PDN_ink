const DashboardApp = (() => {
    // --- 1. CONFIGURACIÓN Y ESTADO ---
    const config = {
        selectors: {
            sidebar: '#sidebar',
            toggleBtn: '#toggleSidebar',
            closeSidebarBtn: '#closeSidebarBtn',
            sectionContent: '.section-content',
            sidebarNavLi: '.sidebar-nav li',
            pageTitleText: '#pageTitleText',
            profileBtn: '#profileBtn',
            profileDropdown: '#profileDropdown',
            moreOptionsBtn: '#moreOptionsBtn',
            moreOptionsDropdown: '#moreOptionsDropdown',
            filterIcon: '.filter-icon',
            filterDropdown: '.filter-dropdown',
            topActionsDropdown: '.top-actions .dropdown-menu',
            searchInput: '#searchInput',
            signModal: '#signModal',
            modalDocTitle: '#modalDocTitle',
            pdfViewer: '.pdf-viewer-container',
            signTabBtn: '#signModal .tab-btn',
            signTabContent: '#signModal .tab-content',
            canvas: '#signatureCanvas',
            sendToSignBtn: '#sendToSignBtn',
            originacionTableCheckboxes: '#originacionTable tbody .custom-checkbox:checked',
            calMonth: '#calMonth',
            calYear: '#calYear',
            calGrid: '#calendarGrid',
            calTabBtn: '.cal-tab-btn',
            calTabContent: '.cal-tab-content',
            dynamicContent: '#dynamic-content',
            // Perfil
            profileTabBtn: '.profile-tab-btn',
            profileContent: '.profile-content',
            newPasswordInput: '#newPassword',
            confirmPasswordInput: '#confirmPassword',
            apiKeyInput: '#apiKeyInput',
            // Documentos
            docTabBtn: '.doc-tab-btn',
            docTabContent: '.doc-tab-content',
            fileInput: '#fileInput',
            fileDropArea: '.file-drop-area',
            participantSearch: '#participantSearch',
            linkParticipantCheck: '#linkParticipantCheck',
            participantSelectContainer: '#participantSelectContainer',
            linkSystemCheck: '#linkSystemCheck',
            systemSelectContainer: '#systemSelectContainer',
            // PARTICIPANTES EN PLANTILLA (NUEVOS SELECTORES)
            linkParticipantCheckTpl: '#linkParticipantCheckTpl',
            participantSelectContainerTpl: '#participantSelectContainerTpl',
            participantSearchTpl: '#participantSearchTpl',
            participantSearchResultsTpl: '#participantSearchResultsTpl',
            // NUEVO SELECTOR PARA BUSQUEDA EN PLANTILLA
            tplFilterBusqueda: '#tplFilterBusqueda',
            // Filtros Documentos
            filterPanel: '#filterPanel',
            toggleFilterBtn: '#toggleFilterBtn',
            filterLevel1: '#filterLevel1',
            filterLevel2: '#filterLevel2',
            filterLevel2Container: '#filterLevel2Container',
            filterStartDate: '#filterStartDate',
            filterEndDate: '#filterEndDate',
            chooseDocsTable: '#chooseDocsTable',
            // Historial
            historySidebar: '#historySidebar',
            historyOverlay: '#historyOverlay',
            historyDocTitle: '#historyDocTitle',
            historyTimeline: '.timeline',
            // Carpetas
            createFolderModal: '#createFolderModal',
            newFolderNameInput: '#newFolderName',
            foldersGrid: '#foldersGrid',
            folderContent: '#folderContent',
            currentFolderName: '#currentFolderName',
            folderFilesBody: '#folderFilesBody'
        },
        docColors: ['#3498db', '#2ecc71', '#e74c3c', '#f1c40f', '#9b59b6', '#1abc9c', '#e67e22'],
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        filterOptions: {
            clasificacion: [
                "Originación del crédito",
                "Capital Humano",
                "Bancos",
                "Operaciones"
            ],
            tipo: [
                "Cédula profesional",
                "Recibo de luz",
                "Recibo de teléfono",
                "Recibo de agua",
                "Recibo de cable",
                "RFC",
                "CURP",
                "F-COM-01-05 determinación del grado de riesgo del acreditado",
                "F-COM-23 formato de información complementaria del cliente pep - alto riesgo"
            ],
            opcion: [
                "Particular",
                "Fiscal",
                "Oficina",
                "Garantía",
                "Pagos parciales a capital 2023",
                "Notas de crédito 2023",
                "Autorización de transferencias 2023",
                "Solicitud de recursos de caja chica y gasolina _ ene2025_macro",
                "Comprobación de gastos de caja chica oct24 _macro",
                "Autorización de disposición de fondeo externo 2023",
                "Matriz de determinación de metodología para el desarrollo de un software"
            ]
        }
    };

    let state = {
        activeFilters: {},
        currentCalDate: new Date(),
        isDrawing: false,
        canvasCtx: null,
        draggingElement: null,
        folders: [
            { id: 1, name: 'Contratos 2025', count: 5 },
            { id: 2, name: 'Proyectos Activos', count: 12 },
            { id: 3, name: 'Documentos Legales', count: 8 }
        ],
        addedParticipants: [] // Nuevo estado para participantes agregados
    };

    // --- 2. UTILIDADES ---
    const Utils = {
        qs: (selector, parent = document) => parent.querySelector(selector),
        qsa: (selector, parent = document) => parent.querySelectorAll(selector),
        parseDate: (dateString) => {
            if (!dateString) return null;
            let date = null;
            if (dateString.includes('-')) {
                const parts = dateString.split('-');
                date = new Date(parts[0], parts[1] - 1, parts[2]);
            } else if (dateString.includes('/')) {
                const parts = dateString.split('/');
                date = new Date(parts[2], parts[1] - 1, parts[0]);
            }
            if (date && !isNaN(date.getTime())) return date;
            return null;
        },
        getDaysDifference: (dateString) => {
            const date = Utils.parseDate(dateString);
            if (!date) return '-';
            const today = new Date();
            today.setHours(0, 0, 0, 0);
            date.setHours(0, 0, 0, 0);
            const diffTime = Math.abs(today - date);
            return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
        },
        generateRandomString: (length) => {
            const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
            let result = '';
            for (let i = 0; i < length; i++) {
                result += chars.charAt(Math.floor(Math.random() * chars.length));
            }
            return result;
        },
        toggleNoRecordsMessage(table) {
            if (!table) return;
            const tbody = table.querySelector('tbody');
            if (!tbody) return;

            const noRecordsRow = tbody.querySelector('.no-records-row');
            
            // Obtener filas de contenido (excluyendo el mensaje)
            const contentRows = Array.from(tbody.rows).filter(r => !r.classList.contains('no-records-row'));
            
            // Contar visibles
            const visibleCount = contentRows.filter(r => r.style.display !== 'none').length;

            if (visibleCount === 0) {
                if (!noRecordsRow) {
                    const headerRow = table.querySelector('thead tr');
                    const colCount = headerRow ? headerRow.cells.length : 1;
                    const newRow = tbody.insertRow();
                    newRow.className = 'no-records-row';
                    newRow.innerHTML = `<td colspan="${colCount}" style="text-align: center; padding: 20px; color: #888;">No hay registros para mostrar</td>`;
                }
            } else {
                if (noRecordsRow) {
                    noRecordsRow.remove();
                }
            }
        },
        injectStyles(styles) {
            const styleSheet = document.createElement("style");
            styleSheet.innerText = styles;
            document.head.appendChild(styleSheet);
        }
    };

    // --- 3. MÓDULOS DE COMPONENTES ---
    const Sidebar = {
        init() {
            const toggleBtn = Utils.qs(config.selectors.toggleBtn);
            const closeSidebarBtn = Utils.qs(config.selectors.closeSidebarBtn);
            const sidebar = Utils.qs(config.selectors.sidebar);

            if (toggleBtn) {
                toggleBtn.addEventListener('click', () => {
                    sidebar.classList.toggle('active');
                });
            }

            if (closeSidebarBtn) {
                closeSidebarBtn.addEventListener('click', () => {
                    sidebar.classList.remove('active');
                });
            }
        }
    };

    const Signature = {
        canvas: null,
        ctx: null,
        isDrawing: false,
        signatureBoxCount: 0,
        
        init() {
            // Inicializar canvas
            this.canvas = Utils.qs(config.selectors.canvas);
            if (this.canvas) {
                this.ctx = this.canvas.getContext('2d');
                this.resizeCanvas();
                window.addEventListener('resize', () => this.resizeCanvas());

                // Eventos de dibujo (Mouse)
                this.canvas.addEventListener('mousedown', (e) => this.startDrawing(e));
                this.canvas.addEventListener('mousemove', (e) => this.draw(e));
                this.canvas.addEventListener('mouseup', () => this.stopDrawing());
                this.canvas.addEventListener('mouseout', () => this.stopDrawing());

                // Eventos de dibujo (Touch)
                this.canvas.addEventListener('touchstart', (e) => this.startDrawing(e));
                this.canvas.addEventListener('touchmove', (e) => this.draw(e));
                this.canvas.addEventListener('touchend', () => this.stopDrawing());
            }

            // Inyectar estilos para los recuadros de firma
            Utils.injectStyles(`
                .signature-drag-box {
                    position: absolute;
                    width: 200px;
                    height: 60px;
                    border: 2px dashed #e74c3c;
                    background: rgba(231, 76, 60, 0.1);
                    color: #c0392b;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-weight: bold;
                    cursor: move;
                    z-index: 100;
                    border-radius: 4px;
                    user-select: none;
                }
                .signature-drag-box:hover {
                    background: rgba(231, 76, 60, 0.2);
                }
                .remove-box-btn {
                    position: absolute;
                    top: -10px;
                    right: -10px;
                    background: #e74c3c;
                    color: white;
                    border: none;
                    border-radius: 50%;
                    width: 20px;
                    height: 20px;
                    cursor: pointer;
                    font-size: 12px;
                    line-height: 1;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                }
                .signature-controls-custom button {
                    margin-left: 10px;
                }
            `);
        },
        resizeCanvas() {
            if (this.canvas) {
                const rect = this.canvas.parentElement.getBoundingClientRect();
                this.canvas.width = rect.width;
                this.canvas.height = 200; // Altura fija o dinámica
                this.ctx.lineWidth = 2;
                this.ctx.lineCap = 'round';
                this.ctx.strokeStyle = '#000';
            }
        },
        getPos(e) {
            const rect = this.canvas.getBoundingClientRect();
            let clientX, clientY;
            if (e.touches && e.touches.length > 0) {
                clientX = e.touches[0].clientX;
                clientY = e.touches[0].clientY;
            } else {
                clientX = e.clientX;
                clientY = e.clientY;
            }
            return {
                x: clientX - rect.left,
                y: clientY - rect.top
            };
        },
        startDrawing(e) {
            e.preventDefault();
            this.isDrawing = true;
            const pos = this.getPos(e);
            this.ctx.beginPath();
            this.ctx.moveTo(pos.x, pos.y);
        },
        draw(e) {
            if (!this.isDrawing) return;
            e.preventDefault();
            const pos = this.getPos(e);
            this.ctx.lineTo(pos.x, pos.y);
            this.ctx.stroke();
        },
        stopDrawing() {
            this.isDrawing = false;
        },
        clearCanvas() {
            if (this.ctx && this.canvas) {
                this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
            }
        },
        openModal(docTitle) {
            const modal = Utils.qs(config.selectors.signModal);
            const titleEl = Utils.qs(config.selectors.modalDocTitle);
            if (modal && titleEl) {
                titleEl.innerText = docTitle;
                modal.style.display = 'flex';
                this.resizeCanvas();
                this.injectControls('insertar'); // Default tab
            }
        },
        closeModal() {
            const modal = Utils.qs(config.selectors.signModal);
            if (modal) modal.style.display = 'none';
            // Limpiar cajas al cerrar
            const boxes = document.querySelectorAll('.signature-drag-box');
            boxes.forEach(b => b.remove());
            this.signatureBoxCount = 0;
            Utils.qsa(config.selectors.originacionTableCheckboxes).forEach(cb => cb.checked = false);
        },
        injectControls(tabName) {
            const tabId = `tab-${tabName}`;
            const tabEl = document.getElementById(tabId);
            if (!tabEl) return;

            // Ocultar botón por defecto si existe (para ambos tabs)
            const defaultBtn = tabEl.querySelector('.btn-sign-confirm');
            if (defaultBtn) defaultBtn.style.display = 'none';

            // Verificar si ya existen los controles personalizados en este tab
            if (tabEl.querySelector('.signature-controls-custom')) return;

            let controlsContainer = document.createElement('div');
            controlsContainer.className = 'signature-controls-custom';
            controlsContainer.style.marginTop = '15px';
            controlsContainer.style.display = 'flex';
            controlsContainer.style.gap = '10px';
            controlsContainer.style.justifyContent = 'flex-end';

            const addBtn = document.createElement('button');
            addBtn.className = 'btn-secondary';
            addBtn.innerHTML = '<i class="fa-solid fa-plus"></i> Agregar Espacio de Firma';
            addBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.addSignatureBox();
            });

            const signBtn = document.createElement('button');
            signBtn.className = 'btn-primary';
            signBtn.innerHTML = '<i class="fa-solid fa-file-signature"></i> Guardar y Firmar';
            signBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.saveSignature();
            });

            controlsContainer.appendChild(addBtn);
            controlsContainer.appendChild(signBtn);

            tabEl.appendChild(controlsContainer);
        },
        addSignatureBox() {
            const viewer = Utils.qs(config.selectors.pdfViewer);
            if (!viewer) {
                Swal.fire('Error', 'No se encontró el visor de documentos', 'error');
                return;
            }

            const box = document.createElement('div');
            box.className = 'signature-drag-box';
            box.innerText = 'Espacio para Firma';
            // Posición inicial aleatoria dentro del visor para que no se encimen
            const top = 50 + (this.signatureBoxCount * 20);
            const left = 50 + (this.signatureBoxCount * 20);
            box.style.top = `${top}px`;
            box.style.left = `${left}px`;
            
            const removeBtn = document.createElement('button');
            removeBtn.className = 'remove-box-btn';
            removeBtn.innerHTML = '&times;';
            removeBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                box.remove();
                this.signatureBoxCount--;
            });
            box.appendChild(removeBtn);

            viewer.appendChild(box);
            this.makeDraggable(box);
            this.signatureBoxCount++;
        },
        makeDraggable(elmnt) {
            let pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
            elmnt.onmousedown = dragMouseDown;

            function dragMouseDown(e) {
                e.preventDefault();
                // Obtener posición inicial del cursor
                pos3 = e.clientX;
                pos4 = e.clientY;
                document.onmouseup = closeDragElement;
                document.onmousemove = elementDrag;
            }

            function elementDrag(e) {
                e.preventDefault();
                // Calcular nueva posición
                pos1 = pos3 - e.clientX;
                pos2 = pos4 - e.clientY;
                pos3 = e.clientX;
                pos4 = e.clientY;
                
                // Establecer nueva posición
                elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
                elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
            }

            function closeDragElement() {
                // Detener movimiento
                document.onmouseup = null;
                document.onmousemove = null;
            }
        },
        saveSignature() {
            const boxes = document.querySelectorAll('.signature-drag-box');
            
            // Si hay firma dibujada en el canvas O hay cajas de firma
            // Permitimos guardar
            let hasDrawing = false;
            // Verificar si el canvas no está vacío (simplificado)
            // Para este ejemplo, asumiremos que si hay cajas es suficiente, 
            // o si el usuario quiere firmar directamente.
            
            if (boxes.length === 0) {
                 Swal.fire({
                    title: 'Confirmar',
                    text: 'No ha colocado espacios de firma. ¿Desea firmar solo con el dibujo?',
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, firmar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        this.processSave(0);
                    }
                });
            } else {
                 this.processSave(boxes.length);
            }
        },
        processSave(boxCount) {
             Swal.fire({
                title: 'Guardando Firma...',
                text: boxCount > 0 ? `Se aplicará la firma en ${boxCount} ubicación(es).` : 'Documento firmado exitosamente.',
                icon: 'success',
                timer: 1500,
                showConfirmButton: false
            }).then(() => {
                this.closeModal();
                this.clearCanvas();
            });
        },
        sendToSign() {
            const checked = Utils.qsa(config.selectors.originacionTableCheckboxes);
            if (checked.length === 0) {
                Swal.fire('Atención', 'Seleccione al menos un documento para enviar a firma.', 'warning');
                return;
            }
            Swal.fire({
                title: 'Enviar a Firma',
                text: `Se enviarán ${checked.length} documentos a firma.`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#151f6d',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Enviar'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire('Enviado', 'Los documentos han sido enviados.', 'success');
                    checked.forEach(c => c.checked = false);
                }
            });
        }
    };
    
    const Calendar = {
        init() {
            const monthSelect = Utils.qs(config.selectors.calMonth);
            const yearSelect = Utils.qs(config.selectors.calYear);
            if (!monthSelect || !yearSelect) return;

            const currentYear = new Date().getFullYear();
            monthSelect.innerHTML = '';
            yearSelect.innerHTML = '';

            for (let i = currentYear - 10; i <= currentYear + 10; i++) {
                yearSelect.add(new Option(i, i));
            }
            config.monthNames.forEach((name, index) => {
                monthSelect.add(new Option(name, index));
            });
            this.updateSelects();
            this.render();
        },
        updateSelects() {
            const monthSelect = Utils.qs(config.selectors.calMonth);
            const yearSelect = Utils.qs(config.selectors.calYear);
            if(monthSelect) monthSelect.value = state.currentCalDate.getMonth();
            if(yearSelect) yearSelect.value = state.currentCalDate.getFullYear();
        },
        changeMonth(delta) {
            state.currentCalDate.setMonth(state.currentCalDate.getMonth() + delta);
            this.updateSelects();
            this.render();
        },
        render() {
            const monthSelect = Utils.qs(config.selectors.calMonth);
            const yearSelect = Utils.qs(config.selectors.calYear);
            const grid = Utils.qs(config.selectors.calGrid);
            if (!grid || !monthSelect || !yearSelect) return;

            state.currentCalDate.setMonth(parseInt(monthSelect.value));
            state.currentCalDate.setFullYear(parseInt(yearSelect.value));
            const year = state.currentCalDate.getFullYear();
            const month = state.currentCalDate.getMonth();
            const firstDay = new Date(year, month, 1).getDay();
            const daysInMonth = new Date(year, month + 1, 0).getDate();

            grid.innerHTML = '';
            ['D', 'L', 'M', 'M', 'J', 'V', 'S'].forEach(day => {
                const dayEl = document.createElement('div');
                dayEl.className = 'cal-day-name';
                dayEl.innerText = day;
                grid.appendChild(dayEl);
            });

            const prevMonthLastDay = new Date(year, month, 0).getDate();
            for (let i = 0; i < firstDay; i++) {
                const dayEl = document.createElement('div');
                dayEl.className = 'cal-day';
                dayEl.style.color = '#ccc';
                dayEl.innerText = prevMonthLastDay - firstDay + 1 + i;
                grid.appendChild(dayEl);
            }

            const today = new Date();
            for (let i = 1; i <= daysInMonth; i++) {
                const dayEl = document.createElement('div');
                dayEl.className = 'cal-day';
                dayEl.innerText = i;
                if (i === today.getDate() && month === today.getMonth() && year === today.getFullYear()) {
                    dayEl.classList.add('active');
                }
                grid.appendChild(dayEl);
            }
        },
        switchTab(button, tabName) {
            const parent = button.closest('.calendar-card');
            Utils.qsa(config.selectors.calTabBtn, parent).forEach(btn => btn.classList.remove('active'));
            button.classList.add('active');

            Utils.qsa(config.selectors.calTabContent, parent).forEach(content => content.classList.remove('active'));
            Utils.qs(`#cal-tab-${tabName}`, parent)?.classList.add('active');
        }
    };

    const Filters = {
        init() {
            const tables = Utils.qsa('table[id]');
            tables.forEach(table => {
                const tableId = table.id;
                state.activeFilters[tableId] = {};
                this.setupTableFilters(table);
                Utils.toggleNoRecordsMessage(table);
            });

            document.removeEventListener('click', this.handleFilterClick);
            document.addEventListener('click', this.handleFilterClick);

            const searchInput = Utils.qs(config.selectors.searchInput);
            if (searchInput) {
                const newInput = searchInput.cloneNode(true);
                searchInput.parentNode.replaceChild(newInput, searchInput);
                newInput.addEventListener('keyup', () => {
                    const globalTerm = newInput.value.toLowerCase();
                    let visibleTable = null;

                    // 1. Buscar si hay una pestaña de documento activa
                    const activeDocTab = document.querySelector('.doc-tab-content.active');
                    if (activeDocTab) {
                        visibleTable = activeDocTab.querySelector('table');
                    }

                    // 2. Si no, buscar la tabla en el contenedor dinámico principal
                    if (!visibleTable) {
                        visibleTable = document.querySelector('#dynamic-content table');
                    }

                    if(visibleTable) {
                        this.runFilters(visibleTable.id);
                    }

                    // 3. Buscar en carpetas (si estamos en la vista de carpetas)
                    const foldersGrid = Utils.qs(config.selectors.foldersGrid);
                    if (foldersGrid && foldersGrid.offsetParent !== null) {
                        Folders.filterFolders(globalTerm);
                    }
                });
            }
        },
        handleFilterClick(e) {
            const filterIcon = e.target.closest('.filter-icon');
            if (filterIcon) {
                e.stopPropagation();
                const th = filterIcon.closest('th');
                const dropdown = th.querySelector('.filter-dropdown');
                const wasOpen = dropdown.classList.contains('show');
                Utils.qsa('.filter-dropdown.show').forEach(d => d.classList.remove('show'));
                if (!wasOpen) dropdown.classList.add('show');
            }
        },
        setupTableFilters(table) {
            const headers = table.querySelectorAll('.filterable-header');
            headers.forEach(th => {
                const colIndex = parseInt(th.getAttribute('data-col'));
                const dropdown = th.querySelector('.filter-dropdown');
                if (!dropdown) return;

                const rows = Array.from(table.querySelectorAll('tbody tr:not(.no-records-row)'));
                const uniqueValues = new Set(rows.map(row => row.cells[colIndex]?.innerText.trim()).filter(Boolean));

                let html = `
                    <div class="filter-options">
                        <button class="sort-btn" onclick="DashboardApp.Filters.sort('${table.id}', ${colIndex}, 'asc')"><i class="fa-solid fa-arrow-down-a-z"></i> A-Z</button>
                        <button class="sort-btn" onclick="DashboardApp.Filters.sort('${table.id}', ${colIndex}, 'desc')"><i class="fa-solid fa-arrow-down-z-a"></i> Z-A</button>
                    </div>
                    <ul class="filter-list">
                        <li><label><input type="checkbox" class="select-all" checked onchange="DashboardApp.Filters.toggleAll(this)"> (Seleccionar todo)</label></li>
                `;

                Array.from(uniqueValues).sort().forEach(val => {
                    html += `<li><label><input type="checkbox" value="${val}" checked class="val-checkbox"> ${val}</label></li>`;
                });

                html += `
                    </ul>
                    <div class="filter-actions">
                        <button class="btn-clear" onclick="DashboardApp.Filters.clear('${table.id}', ${colIndex})">Limpiar</button>
                        <button class="btn-apply" onclick="DashboardApp.Filters.apply('${table.id}', ${colIndex})">Aplicar</button>
                    </div>
                `;
                dropdown.innerHTML = html;
            });
        },
        sort(tableId, colIndex, order) {
            const table = document.getElementById(tableId);
            const tbody = table.querySelector('tbody');
            const rows = Array.from(tbody.querySelectorAll('tr:not(.no-records-row)'));

            rows.sort((a, b) => {
                const valA = a.cells[colIndex]?.innerText.trim().toLowerCase() || '';
                const valB = b.cells[colIndex]?.innerText.trim().toLowerCase() || '';
                return order === 'asc' ? valA.localeCompare(valB) : valB.localeCompare(valA);
            });

            rows.forEach(row => tbody.appendChild(row));
        },
        toggleAll(source) {
            const ul = source.closest('ul');
            ul.querySelectorAll('.val-checkbox').forEach(cb => cb.checked = source.checked);
        },
        apply(tableId, colIndex) {
            const th = document.querySelector(`#${tableId} th[data-col="${colIndex}"]`);
            const selectedValues = Array.from(th.querySelectorAll('.val-checkbox:checked')).map(cb => cb.value);

            if (!state.activeFilters[tableId]) state.activeFilters[tableId] = {};
            state.activeFilters[tableId][colIndex] = selectedValues;

            this.runFilters(tableId);
            th.querySelector('.filter-dropdown').classList.remove('show');
        },
        clear(tableId, colIndex) {
            const th = document.querySelector(`#${tableId} th[data-col="${colIndex}"]`);
            th.querySelectorAll('input[type="checkbox"]').forEach(cb => cb.checked = true);

            if (state.activeFilters[tableId]) delete state.activeFilters[tableId][colIndex];
            this.runFilters(tableId);
            th.querySelector('.filter-dropdown').classList.remove('show');
        },
        runFilters(tableId) {
            const table = document.getElementById(tableId);
            const rows = table.querySelectorAll('tbody tr:not(.no-records-row)');
            const filters = state.activeFilters[tableId] || {};
            const globalTerm = Utils.qs(config.selectors.searchInput)?.value.toLowerCase() || '';

            rows.forEach(row => {
                let isVisible = true;
                for (const colIdx in filters) {
                    const cellValue = row.cells[colIdx]?.innerText.trim();
                    if (!filters[colIdx].includes(cellValue)) {
                        isVisible = false;
                        break;
                    }
                }
                if (isVisible && globalTerm && !row.innerText.toLowerCase().includes(globalTerm)) {
                    isVisible = false;
                }
                row.style.display = isVisible ? '' : 'none';
            });
            Utils.toggleNoRecordsMessage(table);
        }
    };

    const Modal = {
        init() {
            const sendBtn = Utils.qs(config.selectors.sendToSignBtn);
            if (sendBtn) {
                const newBtn = sendBtn.cloneNode(true);
                sendBtn.parentNode.replaceChild(newBtn, sendBtn);

                newBtn.addEventListener('click', () => {
                    const checkedRows = Utils.qsa(config.selectors.originacionTableCheckboxes);
                    if (checkedRows.length === 0) {
                        Swal.fire({
                            icon: 'warning',
                            title: 'No hay documentos seleccionados',
                            text: 'Por favor, seleccione al menos un documento para enviar a firma.',
                            confirmButtonColor: '#151f6d'
                        });
                        return;
                    }
                    const docNames = Array.from(checkedRows).map(cb => cb.closest('tr').cells[7].innerText.trim());
                    this.openSignModal(docNames);
                });
            }
            this.initDragAndDrop();
        },
        openSignModal(docNames) {
            const modal = Utils.qs(config.selectors.signModal);
            const pdfViewer = Utils.qs(config.selectors.pdfViewer, modal);
            if (!modal || !pdfViewer) return;

            pdfViewer.innerHTML = '';
            const docs = Array.isArray(docNames) ? docNames : [docNames];

            docs.forEach((name, index) => {
                const placeholder = document.createElement('div');
                placeholder.className = 'pdf-placeholder';
                placeholder.draggable = true;
                placeholder.style.borderColor = config.docColors[index % config.docColors.length];
                placeholder.dataset.docName = name;
                placeholder.innerHTML = `
                    <button class="remove-doc-btn" onclick="DashboardApp.Modal.removeDocument(this)">&times;</button>
                    <i class="fa-solid fa-file-pdf fa-3x"></i>
                    <p class="doc-title">${name}</p><p>Vista previa del documento</p>`;
                pdfViewer.appendChild(placeholder);
            });

            this.updateTitle();
            modal.style.display = 'flex';
            this.switchSignTab('insertar');
        },
        closeSignModal() {
            // Delegar cierre a Signature para limpiar estado
            DashboardApp.Signature.closeModal();
        },
        removeDocument(btn) {
            btn.closest('.pdf-placeholder').remove();
            this.updateTitle();
        },
        updateTitle() {
            const modal = Utils.qs(config.selectors.signModal);
            const title = Utils.qs(config.selectors.modalDocTitle, modal);
            const docCount = Utils.qsa('.pdf-placeholder', modal).length;
            if (docCount > 0) title.innerText = `Firmando ${docCount} documento(s)`;
            else this.closeSignModal();
        },
        switchSignTab(tabName) {
            const modal = Utils.qs(config.selectors.signModal);
            Utils.qsa(config.selectors.signTabBtn, modal).forEach(btn => btn.classList.remove('active'));
            Utils.qs(`.tab-btn[onclick*="'${tabName}'"]`, modal)?.classList.add('active');
            Utils.qsa(config.selectors.signTabContent, modal).forEach(content => content.classList.remove('active'));
            Utils.qs(`#tab-${tabName}`, modal)?.classList.add('active');
            
            // Inyectar controles de firma al cambiar a CUALQUIER pestaña de firma (insertar o firmar)
            DashboardApp.Signature.injectControls(tabName);
            
            if (tabName === 'firmar') {
                setTimeout(() => this.initCanvas(), 50);
            }
        },
        initCanvas() {
            let canvas = Utils.qs(config.selectors.canvas);
            if (!canvas) return;
            
            // Recrear canvas para limpiar listeners antiguos
            const newCanvas = canvas.cloneNode(true);
            canvas.parentNode.replaceChild(newCanvas, canvas);
            canvas = newCanvas;
            
            // Actualizar referencia en config/state si fuera necesario
            // Aquí usamos la instancia local o reasignamos
            DashboardApp.Signature.canvas = canvas;
            DashboardApp.Signature.ctx = canvas.getContext('2d');
            DashboardApp.Signature.resizeCanvas();
            
            // Re-vincular eventos a través de Signature.init() o manual
            // Para simplificar, delegamos a Signature init events si no están
             canvas.addEventListener('mousedown', (e) => DashboardApp.Signature.startDrawing(e));
             canvas.addEventListener('mousemove', (e) => DashboardApp.Signature.draw(e));
             canvas.addEventListener('mouseup', () => DashboardApp.Signature.stopDrawing());
             canvas.addEventListener('mouseout', () => DashboardApp.Signature.stopDrawing());

             canvas.addEventListener('touchstart', (e) => DashboardApp.Signature.startDrawing(e));
             canvas.addEventListener('touchmove', (e) => DashboardApp.Signature.draw(e));
             canvas.addEventListener('touchend', () => DashboardApp.Signature.stopDrawing());
        },
        clearCanvas() {
            DashboardApp.Signature.clearCanvas();
        },
        initDragAndDrop() {
            const viewer = Utils.qs(config.selectors.pdfViewer);
            if(!viewer) return;

            const newViewer = viewer.cloneNode(true);
            viewer.parentNode.replaceChild(newViewer, viewer);

            newViewer.addEventListener('dragstart', e => {
                if (e.target.classList.contains('pdf-placeholder')) {
                    state.draggingElement = e.target;
                    e.dataTransfer.effectAllowed = 'move';
                    setTimeout(() => e.target.classList.add('dragging'), 0);
                }
            });

            newViewer.addEventListener('dragend', e => {
                if (state.draggingElement) {
                    state.draggingElement.classList.remove('dragging');
                    state.draggingElement = null;
                }
            });

            newViewer.addEventListener('dragover', e => {
                e.preventDefault();
                // Permitir soltar
            });
        }
    };

    const History = {
        init() {
            const overlay = Utils.qs(config.selectors.historyOverlay);
            const closeBtn = Utils.qs('#closeHistoryBtn');
            // Nota: closeHistoryBtn no está en config.selectors pero puede estar en HTML.
            // Si no, usar la clase .close-history-btn
            const closeBtnClass = Utils.qs('.close-history-btn');

            if (overlay) {
                overlay.addEventListener('click', () => this.close());
            }
            if (closeBtnClass) {
                closeBtnClass.addEventListener('click', () => this.close());
            }
        },
        open(docName) {
            const sidebar = Utils.qs(config.selectors.historySidebar);
            const overlay = Utils.qs(config.selectors.historyOverlay);
            const title = Utils.qs(config.selectors.historyDocTitle);
            const timeline = Utils.qs(config.selectors.historyTimeline);

            if (!sidebar || !overlay) return;

            title.innerText = docName;
            timeline.innerHTML = this.generateMockHistory();

            sidebar.classList.add('open');
            overlay.classList.add('show');
        },
        close() {
            const sidebar = Utils.qs(config.selectors.historySidebar);
            const overlay = Utils.qs(config.selectors.historyOverlay);
            if (sidebar) sidebar.classList.remove('open');
            if (overlay) overlay.classList.remove('show');
        },
        generateMockHistory() {
            return `
                <li class="timeline-item created">
                    <div class="timeline-dot"></div>
                    <div class="timeline-content">
                        <span class="timeline-date">01/02/2026 10:30 AM</span>
                        <div class="timeline-title">Documento Creado</div>
                        <div class="timeline-desc">Evelin Ramirez subió el documento.</div>
                    </div>
                </li>
                <li class="timeline-item viewed">
                    <div class="timeline-dot"></div>
                    <div class="timeline-content">
                        <span class="timeline-date">02/02/2026 09:15 AM</span>
                        <div class="timeline-title">Visto por Juan Perez</div>
                        <div class="timeline-desc">El usuario abrió el documento para revisión.</div>
                    </div>
                </li>
                <li class="timeline-item signed">
                    <div class="timeline-dot"></div>
                    <div class="timeline-content">
                        <span class="timeline-date">03/02/2026 04:45 PM</span>
                        <div class="timeline-title">Firmado por Juan Perez</div>
                        <div class="timeline-desc">Firma electrónica registrada exitosamente.</div>
                    </div>
                </li>
            `;
        }
    };

    const Generales = {
        init() {
            this.calculateDays();
            this.setupHistoryButtons();
        },
        calculateDays() {
            const rows = Utils.qsa('#generalesTable tbody tr');
            rows.forEach(row => {
                const dateCell = row.cells[6];
                const daysCell = row.cells[8];
                if(dateCell && daysCell) {
                    const dateStr = dateCell.innerText.trim();
                    const days = Utils.getDaysDifference(dateStr);
                    daysCell.innerText = days;
                }
            });
        },
        setupHistoryButtons() {
            const rows = Utils.qsa('#generalesTable tbody tr');
            rows.forEach(row => {
                const signCell = row.cells[9];
                if (signCell) {
                    signCell.style.cursor = 'pointer';
                    signCell.onclick = () => {
                        const docName = row.cells[1].innerText.trim();
                        History.open(docName);
                    };
                }
            });
        }
    };

    const Vistos = {
        init() {
            this.calculateDays();
            this.setupHistoryButtons();
        },
        calculateDays() {
            const rows = Utils.qsa('#vistosTable tbody tr');
            rows.forEach(row => {
                const dateCell = row.cells[6];
                const daysCell = row.cells[8];
                if(dateCell && daysCell) {
                    const dateStr = dateCell.innerText.trim();
                    const days = Utils.getDaysDifference(dateStr);
                    daysCell.innerText = days;
                }
            });
        },
        setupHistoryButtons() {
            const rows = Utils.qsa('#vistosTable tbody tr');
            rows.forEach(row => {
                const signCell = row.cells[9];
                if (signCell) {
                    signCell.style.cursor = 'pointer';
                    signCell.onclick = () => {
                        const docName = row.cells[1].innerText.trim();
                        History.open(docName);
                    };
                }
            });
        }
    };

    const Profile = {
        init() {
            // Inicializar si es necesario
        },
        switchTab(tabName) {
            Utils.qsa(config.selectors.profileTabBtn).forEach(btn => btn.classList.remove('active'));
            Utils.qs(`.profile-tab-btn[onclick*="'${tabName}'"]`)?.classList.add('active');

            Utils.qsa(config.selectors.profileContent).forEach(content => content.classList.remove('active'));
            Utils.qs(`#profile-tab-${tabName}`)?.classList.add('active');
        },
        validatePassword() {
            const password = Utils.qs(config.selectors.newPasswordInput).value;
            const confirm = Utils.qs(config.selectors.confirmPasswordInput).value;

            const reqs = {
                length: password.length >= 11,
                digit: /\d/.test(password),
                lower: /[a-z]/.test(password),
                upper: /[A-Z]/.test(password),
                special: /[!@#$%^&*(),.?":{}|<>]/.test(password),
                match: password === confirm && password !== ''
            };

            for (const [key, valid] of Object.entries(reqs)) {
                const el = Utils.qs(`#req-${key}`);
                if (el) {
                    const icon = el.querySelector('i');
                    if (valid) {
                        el.classList.add('valid');
                        el.classList.remove('invalid');
                        icon.className = 'fa-solid fa-circle-check';
                    } else {
                        el.classList.remove('valid');
                        el.classList.add('invalid');
                        icon.className = 'fa-solid fa-circle-xmark';
                    }
                }
            }
        },
        generateApiKey() {
            const key = 'pk_live_' + Utils.generateRandomString(24);
            Utils.qs(config.selectors.apiKeyInput).value = key;
        }
    };

    const Documentos = {
        init() {
            // -----------------------------------------------------------
            // INICIALIZACIÓN DE SUBIR DOCUMENTO (ORIGINAL)
            // -----------------------------------------------------------
            const checkParticipant = Utils.qs(config.selectors.linkParticipantCheck);
            const containerParticipant = Utils.qs(config.selectors.participantSelectContainer);
            const checkSystem = Utils.qs(config.selectors.linkSystemCheck);
            const containerSystem = Utils.qs(config.selectors.systemSelectContainer);
            const dropArea = Utils.qs(config.selectors.fileDropArea);
            const fileInput = Utils.qs(config.selectors.fileInput);
            
            // Search de participantes (Subir Doc)
            const participantSearch = Utils.qs(config.selectors.participantSearch);
            if (participantSearch) {
                participantSearch.addEventListener('input', (e) => this.handleParticipantSearch(e, 'participantSearchResults'));
            }

            if (checkParticipant && containerParticipant) {
                checkParticipant.addEventListener('change', (e) => {
                    containerParticipant.style.display = e.target.checked ? 'block' : 'none';
                });
            }

            if (checkSystem && containerSystem) {
                checkSystem.addEventListener('change', (e) => {
                    containerSystem.style.display = e.target.checked ? 'block' : 'none';
                });
            }

            if (dropArea && fileInput) {
                dropArea.addEventListener('click', () => {
                    fileInput.click();
                });
            }

            // -----------------------------------------------------------
            // INICIALIZACIÓN DE DESDE PLANTILLA (NUEVO)
            // -----------------------------------------------------------
            const checkParticipantTpl = Utils.qs(config.selectors.linkParticipantCheckTpl);
            const containerParticipantTpl = Utils.qs(config.selectors.participantSelectContainerTpl);
            const participantSearchTpl = Utils.qs(config.selectors.participantSearchTpl);

            if (participantSearchTpl) {
                participantSearchTpl.addEventListener('input', (e) => this.handleParticipantSearch(e, 'participantSearchResultsTpl'));
            }

            if (checkParticipantTpl && containerParticipantTpl) {
                checkParticipantTpl.addEventListener('change', (e) => {
                    containerParticipantTpl.style.display = e.target.checked ? 'block' : 'none';
                });
            }

            // Filtros de documentos (común a ambas sub-pestañas de "Cargar")
            const filterLevel1 = Utils.qs(config.selectors.filterLevel1);
            if (filterLevel1) {
                // Inicializar estado oculto
                const containerLevel2 = Utils.qs(config.selectors.filterLevel2Container);
                if (containerLevel2) containerLevel2.classList.add('hidden');

                filterLevel1.addEventListener('change', (e) => {
                    this.updateFilterLevel2(e.target.value);
                });
            }
            Utils.toggleNoRecordsMessage(Utils.qs(config.selectors.chooseDocsTable));
            
            this.loadTemplateFilters();
        },
        // --- NUEVAS FUNCIONES PARA DOCUMENTOS ---
        switchSubTab(tabName) {
            Utils.qsa('.sub-tab-btn').forEach(btn => btn.classList.remove('active'));
            Utils.qs(`#btn-sub-${tabName}`).classList.add('active');
            
            Utils.qsa('.sub-tab-content').forEach(content => content.style.display = 'none');
            Utils.qs(`#subtab-content-${tabName}`).style.display = 'block';
        },
        handleParticipantSearch(e, resultsContainerId) {
            const term = e.target.value.toLowerCase();
            const resultsContainer = Utils.qs(`#${resultsContainerId}`);
            
            if (term.length < 2) {
                resultsContainer.style.display = 'none';
                return;
            }

            // LISTA DE PARTICIPANTES DE PRUEBA
            const mockParticipants = [
                'Evelin Ramirez Ramos', 'Aidee Ramirez Ramos', 'Lucias Castellanos', 'Cesar Dorantes', 'Arnulfo Hernandez', 'Juan Perez', 'Maria Garcia'
            ].filter(p => p.toLowerCase().includes(term));
            
            // Determinar qué tabla usar para agregar después
            // Si es Tpl usa la tabla de plantilla, sino la normal
            const targetTableId = resultsContainerId.includes('Tpl') ? 'addedParticipantsTableTpl' : 'addedParticipantsTable';
            const targetInputId = resultsContainerId.includes('Tpl') ? 'participantSearchTpl' : 'participantSearch';

            if (mockParticipants.length > 0) {
                resultsContainer.innerHTML = mockParticipants.map(p => 
                    `<div class="search-result-item" style="padding: 10px; cursor: pointer; border-bottom: 1px solid #eee;" onclick="DashboardApp.Documentos.selectParticipant('${p}', '${resultsContainerId}', '${targetTableId}', '${targetInputId}')">${p}</div>`
                ).join('');
                resultsContainer.style.display = 'block';
            } else {
                resultsContainer.innerHTML = '<div style="padding: 10px; color: #999;">No se encontraron resultados</div>';
                resultsContainer.style.display = 'block';
            }
        },
        selectParticipant(name, resultsContainerId, tableId, inputId) {
            const resultsContainer = Utils.qs(`#${resultsContainerId}`);
            const searchInput = Utils.qs(`#${inputId}`);
            
            resultsContainer.style.display = 'none';
            searchInput.value = '';

            Swal.fire({
                title: 'Seleccionar Rol',
                text: `¿Qué rol tendrá ${name}?`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Firmante',
                cancelButtonText: 'Observador',
                confirmButtonColor: '#3498db',
                cancelButtonColor: '#2ecc71'
            }).then((result) => {
                const role = result.isConfirmed ? 'Firmante' : (result.dismiss === Swal.DismissReason.cancel ? 'Observador' : null);
                if (role) {
                    this.addParticipantToTable(name, role, tableId);
                }
            });
        },
        addParticipantToTable(name, role, tableId = 'addedParticipantsTable') {
            const tbody = Utils.qs(`#${tableId} tbody`);
            const noRecords = tbody.querySelector('.no-records-row');
            if (noRecords) noRecords.remove();

            const row = document.createElement('tr');
            // Nota: Se ajusta el onclick para pasar el ID de la tabla correcto al eliminar
            row.innerHTML = `
                <td>${name}</td>
                <td>${role}</td>
                <td style="text-align: center;">
                    <button class="btn-ghost" style="color: #e74c3c;" onclick="this.closest('tr').remove(); DashboardApp.Documentos.checkEmptyParticipants('${tableId}');"><i class="fa-solid fa-trash"></i></button>
                </td>
            `;
            tbody.appendChild(row);
        },
        checkEmptyParticipants(tableId = 'addedParticipantsTable') {
            const tbody = Utils.qs(`#${tableId} tbody`);
            if (tbody.children.length === 0) {
                tbody.innerHTML = '<tr class="no-records-row"><td colspan="3" style="text-align: center; color: #999;">No se han agregado participantes</td></tr>';
            }
        },
        loadTemplateFilters() {
            const selClasificacion = Utils.qs('#tplFilterClasificacion');
            const selTipo = Utils.qs('#tplFilterTipo');
            const selOpcion = Utils.qs('#tplFilterOpcion');
            const inpBusqueda = Utils.qs('#tplFilterBusqueda'); // Nuevo

            // Habilitar todos los selects inicialmente
            if (selClasificacion) {
                selClasificacion.innerHTML = '<option value="">Seleccione...</option>';
                config.filterOptions.clasificacion.forEach(opt => selClasificacion.add(new Option(opt, opt)));
            }
            if (selTipo) {
                selTipo.innerHTML = '<option value="">Seleccione...</option>';
                config.filterOptions.tipo.forEach(opt => selTipo.add(new Option(opt, opt)));
            }
            if (selOpcion) {
                selOpcion.innerHTML = '<option value="">Seleccione...</option>';
                config.filterOptions.opcion.forEach(opt => selOpcion.add(new Option(opt, opt)));
            }
        },
        toggleTemplateFilter(type) {
            const checkbox = Utils.qs(`#check${type}`);
            const field = type === 'Busqueda' ? Utils.qs(`#tplFilter${type}`) : Utils.qs(`#tplFilter${type}`);
            
            if (checkbox && field) {
                field.disabled = !checkbox.checked;
                field.style.opacity = checkbox.checked ? '1' : '0.6';
                if (!checkbox.checked) {
                    field.value = "";
                }
            }
        },
        searchTemplates() {
            const tbody = Utils.qs('#templatesTable tbody');
            const searchInput = Utils.qs('#tplFilterBusqueda');
            const searchTerm = searchInput && !searchInput.disabled ? searchInput.value.toLowerCase().trim() : '';

            // Validación de longitud mínima solo si el filtro de búsqueda está activo
            if (searchTerm && searchTerm.length < 3) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Búsqueda corta',
                    text: 'Por favor ingrese al menos 3 caracteres para buscar.',
                    confirmButtonColor: '#f39c12'
                });
                return;
            }

            // Simulación de búsqueda con filtro de texto
            Swal.fire({
                title: 'Buscando...',
                timer: 1000,
                didOpen: () => Swal.showLoading()
            }).then(() => {
                let html = '';
                // Datos mockeados
                const data = [
                    { name: "FV-SEG-FISCAL-CARRETERA A SAN ISIDRO MAZACANTEPEC NÚMERO 5215, INTERIOR A 3, COLONIA LOS TEPETATES, MUNICIPIO TLAJOMULCO DE ZÚÑIGA, J-24132.pdf", cla: "Bancos", tip: "RFC", opc: "Pagos parciales  a capital 2023" },
                    { name: "Acta Constitutiva Simplificada", cla: "Bancos", tip: "F-COM-22 formato de determinación de pep", opc: "Autorización de disposición de  fondeo externo 2023" },
                    { name: "FV-SEG-FISCAL-CARRETERA A SAN ISIDRO MAZACANTEPEC NÚMERO 5215, INTERIOR A 3, COLONIA LOS TEPETATES, MUNICIPIO TLAJOMULCO DE ZÚÑIGA, J-24132.pdf", cla: "Legal", tip: "Análisis de crédito", opc: "Formato de solicitud de cambios a la base de datos" }
                ];

                // Filtrar si hay término de búsqueda
                const filteredData = searchTerm ? data.filter(item => 
                    item.name.toLowerCase().includes(searchTerm) || 
                    item.cla.toLowerCase().includes(searchTerm) ||
                    item.tip.toLowerCase().includes(searchTerm) ||
                    item.opc.toLowerCase().includes(searchTerm)
                ) : data.slice(0, 3); // Por defecto mostrar 2

                if (filteredData.length > 0) {
                    filteredData.forEach(item => {
                        html += `
                        <tr>
                            <td>${item.name}</td>
                            <td>${item.cla}</td>
                            <td>${item.tip}</td>
                            <td>${item.opc}</td>
                            <td style="text-align: center;">
                                <button class="btn-primary btn-sm"><i class="fa-solid fa-check"></i> Seleccionar</button>
                            </td>
                        </tr>`;
                    });
                } else {
                    html = '<tr class="no-records-row"><td colspan="4" style="text-align: center; padding: 20px; color: #888;">No se encontraron resultados</td></tr>';
                }
                
                tbody.innerHTML = html;
            });
        },
        // -------------------------------------
        updateFilterLevel2(category) {
            const level2 = Utils.qs(config.selectors.filterLevel2);
            const containerLevel2 = Utils.qs(config.selectors.filterLevel2Container);
            
            if (!level2 || !containerLevel2) return;

            if (!category) {
                containerLevel2.classList.add('hidden');
                return;
            }

            containerLevel2.classList.remove('hidden');
            level2.innerHTML = '<option value="">Seleccione...</option>';

            if (config.filterOptions[category]) {
                config.filterOptions[category].forEach(option => {
                    level2.add(new Option(option, option));
                });
            }
        },
        switchTab(tabName) {
            Utils.qsa(config.selectors.docTabBtn).forEach(btn => btn.classList.remove('active'));
            Utils.qs(`.doc-tab-btn[onclick*="'${tabName}'"]`)?.classList.add('active');
            
            Utils.qsa(config.selectors.docTabContent).forEach(content => content.classList.remove('active'));
            Utils.qs(`#doc-tab-${tabName}`)?.classList.add('active');
        },
        uploadFile() {
            const fileInput = Utils.qs(config.selectors.fileInput);
            if (fileInput.files.length > 0) {
                const file = fileInput.files[0];
                if (file.size > 25 * 1024 * 1024) {
                    Swal.fire('Error', 'El archivo excede el tamaño máximo de 25MB', 'error');
                    return;
                }
                Swal.fire('Éxito', `Archivo "${file.name}" cargado correctamente`, 'success');
            } else {
                Swal.fire('Atención', 'Por favor seleccione un archivo', 'warning');
            }
        },
        toggleFilterPanel(e) {
            e.stopPropagation();
            const panel = Utils.qs(config.selectors.filterPanel);
            if (panel) panel.classList.toggle('show');
        },
        applyFilters(showNotification = true) {
            const level1 = Utils.qs(config.selectors.filterLevel1).value;
            const level2 = Utils.qs(config.selectors.filterLevel2).value;
            const startDateInput = Utils.qs(config.selectors.filterStartDate);
            const endDateInput = Utils.qs(config.selectors.filterEndDate);
            const startDate = startDateInput.value;
            const endDate = endDateInput.value;

            if (startDate && endDate && startDate > endDate) {
                Swal.fire({
                    icon: 'error',
                    title: 'Fecha inválida',
                    text: 'La fecha de inicio no puede ser mayor a la fecha fin.',
                    confirmButtonColor: '#151f6d'
                });
                startDateInput.value = '';
                endDateInput.value = '';
                return;
            }

            const table = Utils.qs(config.selectors.chooseDocsTable);
            const rows = table.querySelectorAll('tbody tr:not(.no-records-row)');
            
            let count = 0;
            rows.forEach(row => {
                let show = true;
                
                // Filtro Cascada
                if (level1) {
                    let colIndex = -1;
                    if (level1 === 'clasificacion') colIndex = 4; // Clasificación
                    else if (level1 === 'tipo') colIndex = 3; // Tipo
                    else if (level1 === 'opcion') colIndex = 2; // Opción

                    if (colIndex !== -1 && level2) {
                        const cellText = row.cells[colIndex].innerText.trim();
                        if (cellText !== level2) show = false;
                    }
                }

                // Filtro Fechas (Columna 5: Fecha)
                if (startDate || endDate) {
                    const start = startDate ? Utils.parseDate(startDate) : null;
                    const end = endDate ? Utils.parseDate(endDate) : null;
                    const rowDate = Utils.parseDate(row.cells[5].innerText.trim()); // Columna Fecha

                    if (rowDate) {
                        if (start && rowDate < start) show = false;
                        if (end && rowDate > end) show = false;
                    }
                }

                row.style.display = show ? '' : 'none';
                if (show) count++;
            });

            Utils.toggleNoRecordsMessage(table);
            Utils.qs(config.selectors.filterPanel).classList.remove('show');

            if (showNotification) {
                if (count > 0) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Filtros Aplicados',
                        text: `Se encontraron ${count} documento(s).`,
                        timer: 1500,
                        showConfirmButton: false
                    });
                } else {
                    Swal.fire({
                        icon: 'info',
                        title: 'No se encontraron documentos',
                        text: 'No se encontraron documentos con los filtros seleccionados.',
                        confirmButtonColor: '#151f6d'
                    });
                }
            }
        },
        clearFilters() {
            // Resetear inputs
            Utils.qs(config.selectors.filterLevel1).value = '';
            Utils.qs(config.selectors.filterLevel2).value = '';
            Utils.qs(config.selectors.filterStartDate).value = '';
            Utils.qs(config.selectors.filterEndDate).value = '';
            Utils.qs(config.selectors.filterPanel).classList.remove('show');

            // Resetear restricciones de fecha
            Utils.qs(config.selectors.filterStartDate).removeAttribute('max');
            Utils.qs(config.selectors.filterEndDate).removeAttribute('min');

            // Ocultar segundo nivel
            const containerLevel2 = Utils.qs(config.selectors.filterLevel2Container);
            if (containerLevel2) containerLevel2.classList.add('hidden');

            // Re-aplicar filtros (sin notificación) para mostrar todo
            this.applyFilters(false);
        }
    };

       const Folders = {
        init() {
            this.renderFolders();
        },
        openCreateModal() {
            const modal = Utils.qs(config.selectors.createFolderModal);
            if (modal) modal.style.display = 'flex';
        },
        closeCreateModal() {
            const modal = Utils.qs(config.selectors.createFolderModal);
            if (modal) modal.style.display = 'none';
            const input = Utils.qs(config.selectors.newFolderNameInput);
            if (input) input.value = '';
        },
        createFolder() {
            const name = Utils.qs(config.selectors.newFolderNameInput).value.trim();
            if (name) {
                const newId = state.folders.length + 1;
                state.folders.push({ id: newId, name: name, count: 0 });
                this.renderFolders();
                
                this.closeCreateModal();
                Swal.fire('Éxito', 'Carpeta creada', 'success');
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Nombre Inválido',
                    text: 'Por favor, ingrese un nombre válido para la carpeta nueva.',
                    confirmButtonColor: '#f39c12'
                });
            }
        },
        renameFolder(id) {
            const folder = state.folders.find(f => f.id === id);
            if (!folder) return;

            Swal.fire({
                title: 'Renombrar carpeta',
                input: 'text',
                inputValue: folder.name,
                showCancelButton: true,
                confirmButtonText: 'Guardar',
                cancelButtonText: 'Cancelar',
                confirmButtonColor: '#151f6d'
            }).then((result) => {
                if (result.isConfirmed && result.value) {
                    folder.name = result.value;
                    this.renderFolders();
                    Swal.fire('Éxito', 'Carpeta renombrada', 'success');
                }
            });
        },
        deleteFolder(id) {
            Swal.fire({
                title: '¿Eliminar carpeta?',
                text: "No podrás revertir esto",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Sí, eliminar'
            }).then((result) => {
                if (result.isConfirmed) {
                    state.folders = state.folders.filter(f => f.id !== id);
                    this.renderFolders();
                    Swal.fire('Eliminado', 'La carpeta ha sido eliminada.', 'success');
                }
            });
        },
        toggleFolderMenu(e, id) {
            e.stopPropagation();
            const menu = document.getElementById(`folder-menu-${id}`);
            const wasOpen = menu.classList.contains('show');
            
            // Cerrar otros menús
            Utils.qsa('.folder-dropdown.show').forEach(d => d.classList.remove('show'));
            
            if (!wasOpen) {
                menu.classList.add('show');
            }
        },
        renderFolders() {
            const grid = Utils.qs(config.selectors.foldersGrid);
            if (!grid) return;
            
            grid.innerHTML = '';
            state.folders.forEach(folder => {
                const card = document.createElement('div');
                card.className = 'folder-card';
                
                card.innerHTML = `
                    <button class="folder-menu-btn" onclick="DashboardApp.Folders.toggleFolderMenu(event, ${folder.id})">
                        <i class="fa-solid fa-ellipsis-vertical"></i>
                    </button>
                    <div id="folder-menu-${folder.id}" class="folder-dropdown">
                        <a href="#" onclick="event.stopPropagation(); DashboardApp.Folders.renameFolder(${folder.id})"><i class="fa-solid fa-pen"></i> Cambiar nombre</a>
                        <a href="#" class="danger" onclick="event.stopPropagation(); DashboardApp.Folders.deleteFolder(${folder.id})"><i class="fa-solid fa-trash"></i> Eliminar</a>
                    </div>
                    <div class="folder-click-area" onclick="DashboardApp.Folders.openFolder(${folder.id})">
                        <i class="fa-solid fa-folder folder-icon"></i>
                        <div class="folder-name">${folder.name}</div>
                        <div class="folder-info">${folder.count} archivos</div>
                    </div>
                `;
                grid.appendChild(card);
            });
        },
        openFolder(id) {
            const folder = state.folders.find(f => f.id === id);
            if (!folder) return;

            const grid = Utils.qs(config.selectors.foldersGrid);
            const content = Utils.qs(config.selectors.folderContent);
            const title = Utils.qs(config.selectors.currentFolderName);
            const tbody = Utils.qs(config.selectors.folderFilesBody);
            
            if (grid && content && title) {
                grid.style.display = 'none';
                content.style.display = 'block';
                title.innerText = folder.name;

                // Simular archivos
                tbody.innerHTML = `
                    <tr>
                        <td><div class="doc-icon-wrapper"><img src="img/pdf.jpg" alt="PDF"></div></td>
                        <td>Documento Ejemplo 1</td>
                        <td>Contrato</td>
                        <td>12/02/2026</td>
                        <td>1.5 MB</td>
                        <td>
                            <button class="table-action-btn" title="Ver"><i class="fa-solid fa-eye"></i></button>
                        </td>
                    </tr>
                `;
            }
        },
        showFoldersList() {
            const grid = Utils.qs(config.selectors.foldersGrid);
            const content = Utils.qs(config.selectors.folderContent);
            if (grid && content) {
                grid.style.display = 'grid';
                content.style.display = 'none';
            }
        }
    };

    const Dropdowns = {
        init() {
            const profileBtn = Utils.qs(config.selectors.profileBtn);
            const profileDropdown = Utils.qs(config.selectors.profileDropdown);
            this.setup(profileBtn, profileDropdown);

            const moreOptionsBtn = Utils.qs(config.selectors.moreOptionsBtn);
            const moreOptionsDropdown = Utils.qs(config.selectors.moreOptionsDropdown);
            this.setup(moreOptionsBtn, moreOptionsDropdown);

            document.addEventListener('click', (e) => {
                if (!e.target.closest('.dropdown-container')) {
                    Utils.qsa(config.selectors.topActionsDropdown).forEach(d => d.classList.remove('show'));
                }
                if (!e.target.closest('.filter-dropdown') && !e.target.closest('.filter-icon')) {
                    Utils.qsa('.filter-dropdown.show').forEach(d => d.classList.remove('show'));
                }
                if (!e.target.closest('.action-menu-container')) {
                    Utils.qsa('.action-dropdown.show').forEach(d => d.classList.remove('show'));
                    Utils.qsa('.table-action-btn.active').forEach(b => b.classList.remove('active'));
                }
                if (!e.target.closest('.folder-card')) {
                    Utils.qsa('.folder-dropdown.show').forEach(d => d.classList.remove('show'));
                }
            });
        },
        setup(btn, dropdown) {
            if (btn && dropdown) {
                btn.addEventListener('click', (e) => {
                    e.stopPropagation();
                    // Cerrar otros
                    Utils.qsa(config.selectors.topActionsDropdown).forEach(d => {
                        if (d !== dropdown) d.classList.remove('show');
                    });
                    dropdown.classList.toggle('show');
                });
            }
        }
    };

    const TableActions = {
        init() {
            const actionBtns = Utils.qsa('.table-action-btn[title="Acciones"]');
            actionBtns.forEach(btn => {
                const newBtn = btn.cloneNode(true);
                btn.parentNode.replaceChild(newBtn, btn);

                newBtn.addEventListener('click', (e) => {
                    e.stopPropagation();
                    const container = newBtn.closest('.action-menu-container');
                    const dropdown = container.querySelector('.action-dropdown');
                    const wasOpen = dropdown.classList.contains('show');

                    Utils.qsa('.action-dropdown.show').forEach(d => d.classList.remove('show'));
                    Utils.qsa('.table-action-btn.active').forEach(b => b.classList.remove('active'));

                    if (!wasOpen) {
                        dropdown.classList.add('show');
                        newBtn.classList.add('active');
                    }
                });
            });
        }
    };

    const Navigation = {
        init() {
            const sidebar = Utils.qs(config.selectors.sidebar);
            const toggleBtn = Utils.qs(config.selectors.toggleBtn);
            const closeBtn = Utils.qs(config.selectors.closeSidebarBtn);

            if (toggleBtn && sidebar) {
                toggleBtn.addEventListener('click', () => {
                    if (window.innerWidth <= 992) sidebar.classList.toggle('active');
                    else sidebar.classList.toggle('collapsed');
                });
            }
            if (closeBtn && sidebar) {
                closeBtn.addEventListener('click', () => sidebar.classList.remove('active'));
            }

            const myProfileLink = Utils.qs('#profileDropdown a[href="#"]');
            if (myProfileLink) {
                myProfileLink.addEventListener('click', (e) => {
                    e.preventDefault();
                    this.loadPage('perfil');
                    Utils.qs(config.selectors.profileDropdown).classList.remove('show');
                });
            }

            const newDocLink = Utils.qs('#moreOptionsDropdown a');
            if (newDocLink) {
                newDocLink.addEventListener('click', (e) => {
                    e.preventDefault();
                    this.loadPage('documentos');
                    Utils.qs(config.selectors.moreOptionsDropdown).classList.remove('show');
                });
            }
        },
        async loadPage(pageName, menuElement) {
            const contentContainer = Utils.qs(config.selectors.dynamicContent);

            try {
                const response = await fetch(`${pageName}`);
                if (!response.ok) throw new Error('Error al cargar la página');
                const html = await response.text();
                contentContainer.innerHTML = html;

                if (menuElement) {
                    Utils.qsa(config.selectors.sidebarNavLi).forEach(li => li.classList.remove('active'));
                    menuElement.classList.add('active');
                    const titleText = menuElement.querySelector('.menu-text').innerText;
                    const pageTitle = Utils.qs(config.selectors.pageTitleText);
                    if (pageTitle) pageTitle.innerText = titleText;
                } else if (pageName === 'perfil') {
                    Utils.qsa(config.selectors.sidebarNavLi).forEach(li => li.classList.remove('active'));
                    const pageTitle = Utils.qs(config.selectors.pageTitleText);
                    if (pageTitle) pageTitle.innerText = 'Mi Perfil';
                } else if (pageName === 'documentos') {
                    Utils.qsa(config.selectors.sidebarNavLi).forEach(li => li.classList.remove('active'));
                    const pageTitle = Utils.qs(config.selectors.pageTitleText);
                    if (pageTitle) pageTitle.innerText = 'Nuevo Documento';
                }

                if (window.innerWidth <= 992) {
                    Utils.qs(config.selectors.sidebar).classList.remove('active');
                }

                if (pageName === 'dashboard') {
                    Calendar.init();
                } else if (pageName === 'originacion') {
                    Modal.init();
                    Filters.init();
                } else if (pageName === 'generales') {
                    Generales.init();
                    Filters.init();
                    TableActions.init();
                } else if (pageName === 'vistos') {
                    Vistos.init();
                    Filters.init();
                    TableActions.init();
                } else if (pageName === 'perfil') {
                    Profile.init();
                } else if (pageName === 'documentos') {
                    Documentos.init();
                    Filters.init();
                } else if (pageName === 'carpetas') {
                    Folders.init();
                }

            } catch (error) {
                console.error(error);
                contentContainer.innerHTML = '<p>Error al cargar el contenido.</p>';
            }
        }
    };

    // --- 5. INICIALIZACIÓN ---
    const init = () => {
        Navigation.init();
        Dropdowns.init();
        Sidebar.init();
        Signature.init();
        Calendar.init();
        Profile.init();
        Documentos.init();
        History.init();
        Folders.init();
        TableActions.init();

        // Cargar página inicial por defecto
        const firstMenu = Utils.qs(config.selectors.sidebarNavLi);
        if (firstMenu) {
            const sectionId = firstMenu.getAttribute('onclick').match(/'([^']+)'/)[1];
            Navigation.loadPage(sectionId, firstMenu);
        }
    };

    // --- API PÚBLICA ---
    return {
        init,
        Navigation,
        Modal,
        Calendar,
        Filters,
        Profile,
        Documentos,
        TableActions,
        History,
        Folders,
        Utils,
        Signature
    };

})();

document.addEventListener('DOMContentLoaded', () => {
    DashboardApp.init();
});

// Funciones globales que aún son llamadas por `onclick` en el HTML.
function sortTable(tableId, colIndex, order) { /* ... */ }
function toggleAllFilters(source) { /* ... */ }
function applyFilter(tableId, colIndex) { /* ... */ }
function clearFilter(tableId, colIndex) { /* ... */ }