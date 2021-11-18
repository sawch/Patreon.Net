using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    /// <summary>
    /// A file uploaded to patreon.com, usually an image.
    /// </summary>
    public class Media : PatreonResource
    {
        /// <summary>
        /// When the file was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// The URL to download this media. Valid for 24 hours.
        /// </summary>
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }
        /// <summary>
        /// File name.
        /// </summary>
        [JsonProperty("file_name")]
        public string Filename { get; set; }
        /// <summary>
        /// The resized image URLs for this media. Valid for 2 weeks.
        /// </summary>
        [JsonProperty("image_urls")]
        public object ImageUrls { get; set; }
        /// <summary>
        /// Metadata related to the file. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("metadata")]
        public object Metadata { get; set; }
        /// <summary>
        /// Mimetype of uploaded file, eg: "application/jpeg".
        /// </summary>
        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }
        /// <summary>
        /// Ownership id (See also <see cref="OwnerType"/>).
        /// </summary>
        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }
        /// <summary>
        /// Ownership relationship type for multi-relationship medias.
        /// </summary>
        [JsonProperty("owner_relationship")]
        public string OwnerRelationship { get; set; }
        /// <summary>
        /// Type of the resource that owns the file.
        /// </summary>
        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }
        /// <summary>
        /// Size of file in bytes.
        /// </summary>
        [JsonProperty("size_bytes")]
        public int SizeBytes { get; set; }
        /// <summary>
        /// The state of the file.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
        /// <summary>
        /// When the upload URL expires.
        /// </summary>
        [JsonProperty("upload_expires_at")]
        public DateTimeOffset UploadExpiresAt { get; set; }
        /// <summary>
        /// All the parameters that have to be added to the upload form request.
        /// </summary>
        [JsonProperty("upload_parameters")]
        public object UploadParameters { get; set; }
        /// <summary>
        /// The URL to perform a POST request to in order to upload the media file.
        /// </summary>
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }
    }
}
