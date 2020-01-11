///<reference path="../Agencies/Api.ts"/>
///<reference path="../Common/Controllers.ts"/>

/* @ngInject */
class WinServicesController extends VmController {
    services: Array<WinService>;
    currentService: WinService;
    currentServiceState = CurrentServiceState.oldState;

    loadingIndicator = new ProgressIndicator();

    constructor(operationsExecutor: IOperationExecutor, globalIndicator: IProgressIndicator, private modalDialogsService: ModalDialogsService) {
        super(operationsExecutor, globalIndicator);

        this.services = new Array();
        this.loadWinServices();
    }

    loadWinServices(): void {
        this.services = [];

        this.execute<WinService[]>(new GetWinServicesListOperation(), this.loadingIndicator)
            .then(d => {
                this.services = d.map(x => new WinService(x.name, x.statusId));
            });
    }

    updateWinServices($event, service: WinService): void {
        this.modalDialogsService.confirm('Do you want to change ' + '"' + service.name + '"' + ' status?')
            .then(r => {
                if (r) {
                    this.currentServiceState = CurrentServiceState.newState;
                    service.statusId = service.selectedStatus.id;
                    this.execute(new ChangeWinServiceOperation(service), this.loadingIndicator).then(() => {this.loadWinServices();});
                } else {
                    this.cancelServiceState(new WinService(this.currentService.name, this.currentService.statusId));
                }
            });
    }

    rememberServiceState(service: WinService): void {
        if (this.currentServiceIsReady(service)) {
            this.currentServiceState = CurrentServiceState.oldState;
            this.currentService = new WinService(service.name, service.statusId);
        }
    }

    cancelServiceState(service: WinService): void {
        this.services.forEach((serviceItem, i, services) => {
            if (serviceItem.name === service.name) {
                services[i] = service;
            }
        });
    }

    currentServiceIsReady(service: WinService): boolean {
        return (this.currentService == undefined ||
            this.currentService.name !== service.name ||
            this.currentServiceState === CurrentServiceState.newState);
    }
}

/* @ngInject */

app.controller('WinServicesController', WinServicesController);