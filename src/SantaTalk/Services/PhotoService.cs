using System;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace SantaTalk
{
    public class PhotoService
    {
        private PermissionStatus cameraOK;
        private PermissionStatus storageOK;

        private async Task Init()
        {
            await CrossMedia.Current.Initialize();

            cameraOK = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            storageOK = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraOK != PermissionStatus.Granted || storageOK != PermissionStatus.Granted)
            {
                var status = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraOK = status[Permission.Camera];
                storageOK = status[Permission.Storage];
            }
        }

        public async Task<MediaFile> TakePhoto()
        {
            await Init();

            MediaFile file = null;

            if (cameraOK == PermissionStatus.Granted
                && storageOK == PermissionStatus.Granted
                && CrossMedia.Current.IsCameraAvailable
                && CrossMedia.Current.IsTakePhotoSupported)
            {
                var options = new StoreCameraMediaOptions()
                {
                    Directory = "SantaTalk",
                    Name = $"{Guid.NewGuid()}.jpg",
                    SaveToAlbum = true
                };

                file = await CrossMedia.Current.TakePhotoAsync(options);
            }

            return file;
        }

        public async Task<MediaFile> ChoosePhoto()
        {
            MediaFile file = null;

            if (CrossMedia.Current.IsPickPhotoSupported)
                file = await CrossMedia.Current.PickPhotoAsync();

            return file;
        }
    }
}