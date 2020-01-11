class WinService {
    name: string;
    statusId: number;
    selectedStatus: WinStatus;
    potentialStatus: WinStatus[];

    constructor(name: string, statusId: number) {
        this.name = name;
        this.statusId = statusId;

        this.setSelectedStatus();
        this.setPotentialStatus();
    }

    setSelectedStatus(): void {
        this.selectedStatus =
            new WinStatus(PotentialStatusDescription.getDescriptionById(this.statusId), this.statusId);
    }

    applySelectedStatus(): void {
        this.statusId = this.selectedStatus.id;
    }

    setPotentialStatus(): void {
        this.potentialStatus = [];

        if (this.statusId == PotentialStatusId.running) {
            this.potentialStatus.push(new WinStatus(PotentialStatusDescription.stopped, PotentialStatusId.stopped));

            this.potentialStatus.push(new WinStatus(PotentialStatusDescription.restart, PotentialStatusId.restart));

            this.potentialStatus.push(new WinStatus(PotentialStatusDescription.running, PotentialStatusId.running));
        }
        else if (this.statusId == PotentialStatusId.stopped) {
            this.potentialStatus.push(
                new WinStatus(PotentialStatusDescription.running, PotentialStatusId.running));

            this.potentialStatus.push(
                new WinStatus(PotentialStatusDescription.stopped, PotentialStatusId.stopped));
        }
    }
}

class WinStatus {
    description: string;
    id: number;

    constructor(description: string, id: number) {
        this.description = description;
        this.id = id;
    }
}

class PotentialStatusDescription {
    static running = "Running";
    static stopped = "Stopped";
    static restart = "Restart";

    static getDescriptionById(id: number): string {
        switch (id) {
        case PotentialStatusId.running:
            return PotentialStatusDescription.running;
        case PotentialStatusId.stopped:
            return PotentialStatusDescription.stopped;
        case PotentialStatusId.restart:
            return PotentialStatusDescription.restart;
        default:
            return "incorrect status id";
        }
    }
}

class PotentialStatusId {
    static running = 301;
    static stopped = 302;
    static restart = 303;
}

class CurrentServiceState {
    static oldState = 0;
    static newState = 1;
}