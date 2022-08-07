using Newtonsoft.Json;

namespace ByLearning.SSO.Application.ViewModels
{
    public class ProfilePictureViewModel : FileUploadViewModel
    {
        [JsonIgnore]
        public string Picture { get; set; }
    }

}
