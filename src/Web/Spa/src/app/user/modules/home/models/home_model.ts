import { CategoryPublicModel } from '@user/models/catalog/category_public_model';

export class CategoryHomeModel {
    public title: string;
    public description: string;
    public metaTitle: string;
    public metaDescription: string;
    public metaKeywords: string;
    public categories: CategoryPublicModel[];
}
