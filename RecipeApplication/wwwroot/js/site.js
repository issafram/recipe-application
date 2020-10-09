function refreshFavoritesMenu(favoritesArray) {
    $("#favorites-menu").html("");
    if (favoritesArray.length === 0 || apiData.length === 0) {
        $("#favorites-menu").addClass("d-none");
    }
    $.each(favoritesArray, function (index, obj) {
        if (apiData.some(item => item.id === obj.id)) {
            var favoriteAnchor = favoritesAnchorTemplate.replace(/__REPLACE__/g, obj.id).replace(/__REPLACE_TITLE__/g, obj.title);
            $("#favorites-menu").append(favoriteAnchor);
            $("#favorites-menu").removeClass("d-none");
        }
    });
}

function LoadData(data) {
    apiData = data;
    var favorites = JSON.parse(localStorage.getItem("recipe-favorites"));
    $.each(favorites, function (index, obj) {
        if (data.some(item => item.id === obj.id)) {
            $("td[data-recipe-id='" + obj.id + "']").find("button.not-bookmarked").addClass("d-none");
            $("td[data-recipe-id='" + obj.id + "']").find("button.bookmarked").removeClass("d-none");

            var favoriteAnchor = favoritesAnchorTemplate.replace(/__REPLACE__/g, obj.id).replace(/__REPLACE_TITLE__/g, obj.title);
            $("#favorites-menu").append(favoriteAnchor);
        }
    });
}

function GetData() {
    $.getJSON("/api/recipes", function (data) {
        $.each(data, function (item) {
            data.push(item);
        });

        LoadData(data);
    });
}