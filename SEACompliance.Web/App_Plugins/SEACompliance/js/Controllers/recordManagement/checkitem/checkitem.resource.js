function checkitemResource($http) {
    return {
        getById: function (id) {
            return $http.get("backoffice/SEACompliance/RICheckItemApi/GetById?id=" + id);
        },
        createCheckItem: function (checkitem) {
            return $http.post("backoffice/SEACompliance/RICheckItemApi/CreateCheckItem", angular.toJson(checkitem));
        },
        getAll: function () {
            return $http.get("backoffice/SEACompliance/RICheckItemApi/GetAll");
        },
        updateCheckItem: function (checkitem) {
            return $http.post("backoffice/SEACompliance/RICheckItemApi/UpdateCheckItem", angular.toJson(checkitem));
        },
        deleteCheckItemById: function (id) {
            return $http.get("backoffice/SEACompliance/RICheckItemApi/GetDeleteCheckItemById?id=" + id);
        }
    };
}
angular.module("umbraco.resources").factory("checkitemResource", checkitemResource);