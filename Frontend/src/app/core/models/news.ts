export interface News {
  id: string;
  date: string;
  titleShort: string;
  title: string;
  descriptionFirstPart: string;
  subTitle: string;
  descriptionSecondPart: string;
  photos?: string[];
  conclusion?: string;
}
