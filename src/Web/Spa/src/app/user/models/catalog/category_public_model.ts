import { CategorySimpleModel } from '@user/models/catalog/category_simple_model';

export class CategoryPublicModel {
    public title: string;
    public description: string;
    public metaTitle: string;
    public metaDescription: string;
    public metaKeywords: string;
    public categoryBreadcrumb: CategorySimpleModel[];
}
