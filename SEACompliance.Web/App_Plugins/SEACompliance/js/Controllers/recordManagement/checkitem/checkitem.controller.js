function checkItemController($scope, checkitemResource, $location, notificationsService) {
    $scope.getAllCheckItem = function () {
        checkitemResource.getAll().then(function (response) {
            if (response.data.status === "success") {
                $scope.checkItems = response.data.data;
            }
        });
    }

    $scope.location = function (id) {
        if (id == undefined) {
            id = -1;
        }
        $location.path('SEACompliance/SEACompliance/RICheckItemEdit/' + id);
    }

    $scope.deleteCheckItemById = function (checkitem) {
        if (confirm('Do you want to delete the checkitem ?')) {
            checkitemResource.deleteCheckItemById(checkitem.DocID).then(function (response) {
                if (response.data.status === "success") {
                    $scope.getAllRecord();
                    notificationsService.success("Success", checkitem.DocID + " has been deleted! ");
                } else {
                    notificationsService.error("Failed", checkitem.DocID + " deleted failed! ");
                }
            });
        }
    }
}

angular.module("umbraco").controller('checkItemController', checkItemController);
