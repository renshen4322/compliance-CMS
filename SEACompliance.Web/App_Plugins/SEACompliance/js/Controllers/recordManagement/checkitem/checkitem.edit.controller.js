function checkItemEditController($scope, checkItemResource, $routeParams, $location, notificationsService, overlayHelper, formHelper, eventsService) {
    $scope.checkItemPageModel = { DocID: ""};
    $scope.validationFields = {
    };

    if ($routeParams.id > 0) {
        getById($routeParams.id);
    } else {
        generateEditorProperties();
    }
    function getById(id) {
        checkItemResource.getById(id).then(function (response) {
            if (response.data.data) {
                $scope.checkItemPageModel = response.data.data;   
                generateEditorProperties();
            }
        });
    }

    function generateEditorProperties() {
        var propertyConfig = {
            editor: {
                toolbar: ["code", "undo", "redo", "cut", "bold", "italic", "underline", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "table", "link", "fontselect", "fontsizeselect", "forecolor"],
                stylesheets: [],
                dimensions: { height: 240, width: '90%' }
            }
        };

        $scope.checkItemContentProperties = [{
            label: 'RichText',
            description: 'RichText',
            view: 'rte',
            config: propertyConfig,
            hideLabel: true,
            value: $scope.checkItemPageModel.CheckItemContent ? $scope.checkItemPageModel.CheckItemContent : ""
        }];

        $scope.penaltyProperties = [{
            label: 'RichText',
            description: 'RichText',
            view: 'rte',
            config: propertyConfig,
            hideLabel: true,
            value: $scope.checkItemPageModel.Penalty ? $scope.checkItemPageModel.Penalty : ""
        }];

        $scope.reasonCodesProperties = [{
            label: 'RichText',
            description: 'RichText',
            view: 'rte',
            config: propertyConfig,
            hideLabel: true,
            value: $scope.checkItemPageModel.ReasonCodes ? $scope.checkItemPageModel.ReasonCodes : ""
        }];
    }

    $scope.SaveCheckItem = function () {
        $scope.checkItemPageModel.DocID = $scope.checkItemPageModel.DocID;
        $scope.checkItemPageModel.Title = $scope.checkItemContentProperties[0].value;
        $scope.checkItemPageModel.Risk = $scope.penaltyProperties[0].value;
        $scope.checkItemPageModel.ReferenceDocument = $scope.reasonCodesProperties[0].value;
        $scope.checkItemPageModel.ID = $routeParams.id;

        if ($routeParams.id > 0) {
            checkItemResource.updateCheckItem($scope.checkItemPageModel).then(function (response) {
                if (response.data.status === "success") {
                    getById($routeParams.id);
                    notificationsService.success("Success", $scope.checkItemPageModel.DocID + "has been updated!");
                } else {
                    getById($routeParams.id);
                    notificationsService.error("Failed", response.data.code);
                }
            });
        } else {
            checkItemResource.createCheckItem($scope.checkItemPageModel).then(function (response) {
                if (response.data.status === "success") {
                    $location.path('SEACompliance/SEACompliance/RICheckItemEdit/' + response.data.data.DocID);
                    notificationsService.success("Success", "Topic has been created!");
                } else {
                    notificationsService.error("Failed", response.data.code);
                }
            });
        }
    }
}

angular.module("umbraco").controller('checkItemEditController', checkItemEditController);
