export enum Gender {
    Male,
    Female
}

export function mapStringToGender(value: string) {
    switch(value) {
        case 'male':
            return Gender.Male;
        case 'female':
            return Gender.Female;
        default:
            throw new Error('Unvalid gender string value.');
    }
}

export function mapGenderToString(gender: Gender) {
    switch(gender) {
        case Gender.Male:
            return 'male';
        case Gender.Female:
            return 'female';
        default:
            throw new Error('Unvalid gender value.');
    }
}