using System;
using System.Collections.Generic;
using System.Text;

namespace Bahrin.Harbour.Helper
{
    public class Constants
    {
        public const Int32 EmailLength = 255;
        public const string WrongEmail = "Wrong Email.";
        public const string EmailNotValid = "Enter valid email.";
        public const string EmailInvailedLength = "Enter valid email length.";
        public const string WrongPhoneNo = "Wrong Phone Number";
        public const string PhoneNoNotValid = "Enter valid Phone number.";
        public const string PhoneNoRequired = "Phone number field is required.";
        public const string PhoneNoInvailedLength = "Enter valid Phone number length.";
        public const string WrongCredential = "We couldn’t find an account with that email address or phone number.";
        public const string AccountDisable = "Your account has been disabled. Please contact the System Administrator.";
        public const string AccountDeleted = "Your account has been deleted.";
        public const string OopsSomethingWrong = "!Oops something went wrong. Please try again.";
        public const string AllreadyRedeemed = "The code you entered has already been redeemed on a different account.";
        public const string WrongOldPassword = "Old password does not match.";
        public const string EmailOrPhone = "Enter valid email Or Phone.";
        public const string Password = "Enter valid password.";
        public const string Name = "Enter your name";
        public const string DateofBirth = "Enter your date of Birth.";
        public const string FutureDateofBirth = "Date of birth can’t be in future.";
        public const string GoogleId = "Enter your googleId";
        public const string FacebookId = "Enter your facebookId";
        public const string email = "Enter your Email";
        public const string AddCollection = "Collection created";
        public const string CollectionAlreadyExists = "Collection name already exists";
        public const string DeleteCollection = "Collection deleted";
        public const string BookAddIntoCollection = "Book added to collection";
        public const string BookAlreadyIntoCollection = "Book already added to the Collection @@COLLECTION-NAME";
        public const string BookRemoveFromCollection = "Book removed from collection";
        public const string SubscriptionCancelled = "Your subscription has been successfully cancelled.";

        public const string EmailExists = "Email is already registered, please enter another email id";
        public const string UserNameExists = "User name is already register, please enter another user name";
        public const string PhoneNumberExists = "Phone number is already registerd, please enter another phone number";
        public const string InvalidEmailPhone = "Invalid email or phone number";
        public const string NotExistEmailPhone = "The email or mobile number you entered did not match our records. Please check & try again";

        public const string successfullyLogin = "Logged in";
        public const string successfullyLogout = "Logged out";
        public const string successfullyAdd = "Successfully added";
        public const string successfullyRedeemed = "Redeemed";
        public const string successfullySaved = "Saved";
        public const string favoritesSaved = "Added to favorites";
        public const string SavedToYourLibrary  = "Saved to your library ";
        public const string SavedToYourQueue = "Saved to your queue ";
        public const string successfullyRegister = "Registered";
        public const string successfullyRemoved = "Removed";
        public const string favoritesRemoved = " Removed from favorites";
        public const string successfullyRemovedFromLibrary = "Removed from your library";
        public const string successfullyRemovedFromQueue = "Removed from your queue";
        public const string successfullyDeleted = " Deleted";
        public const string successfullyDeactivated = " Deactivated";
        public const string successfullyPublished = " Published";
        public const string successfullyUnpublished = "Unpublished";
        public const string successfullyNotificationSent = "Notification sent";
        public const string successfullyEmailSent = "Email sent";
        public const string successfullyNotificationDeleted = "Notification All Clear";
        public const string successfullyUpdateNotification = "Marked as read";
        public const string NotificationCleared = "All notification cleared.";

        public const string LinkExpire = "Link Expired.";
        public const string CodeExpire = "The Redeem code you entered has expired.";
        public const string InvalidCode = "Please enter valid code";
        public const string successfullyUpdate = "Updated";
        public const string passwordSuccessfullyUpdate = "Password updated ";
        public const string PasswordMismatch = "Password mismatch";
        public const string EndDateGraterThanStartDate = "The end date should be greater than the start date.";

        public const string successfullyReset = "Password reset successful";
        public const string successfullyChange = "Password Changed";
        public const string Listened = "Listened";

        public const string DefaultUserImage = "/images/blank-person.png";
        public const string DefaultCategoryImage = "/images/Category/Category.jpg";
        public const string DefaultSubcategoryImage = "/images/Category/Category.jpg";
        public const string DefaultBookImage = "/images/book/DefaultBook.png";
        public const string DefaultSubscriptionImage = "/images/Subscription/Subscription.jpg";
        public const string DefaultCouponImage = "/images/Subscription/Coupon.png";
        public const string DefaultStartedScreen = "/images/Subscription/Subscription.jpg";
        public const string categoryList = "Category List";
        public const string subcategoryList = "subcategory List";
        public const string SubscriptionList = "Subscription List";
        public const string bookList = "Book List";
        public const string LangingPageBookList = "Most Popular Books";
        public const string PodcastList = "Podcast List";
        public const string PodcastEpisodeList = "Podcast episode List";
        public const string CollectionList = "Collection List";
        public const string bookInfo = "Book Info";
        public const string chapterList = "Chapter List";
        public const string ReviewList = "Review List";
        public const string ContentList = "Content List";
        public const string NotificationList = "Notification List";
        public const string CouponDetail = "Coupon applied.";
        public const string CouponNotValid = "The coupon code you entered is not valid.";
        public const string CouponAlreadyUsed = "The coupon code already used by you.";

        public const string CategorySaved = "Saved";
        public const string UserDetail = "User detail";
        public const string DataNotFound = "Detail is not found";

        public const string CompanySetting = "CompanySetting";
        public const string ApplicationSetting = "ApplicationSetting";
        public const string AmazonCognitoSetting = "AmazonCognitoSetting";
        public const string AWSConfiguration = "AWSConfiguration";
        public const string MongoDbSetting = "MongoDbSetting";
        public const string CurrencySetting = "CurrencySetting";
        public const string DateFormatSetting = "DateFormatSetting";
        public const string DeveloperSetting = "DeveloperSetting";
        public const string SMTPSetting = "SMTPSetting";
        public const string NotificationSetting = "NotificationSetting";
        public const string RazorPaySetting = "RazorPaySetting";
        public const string FcmNotificationSetting = "FcmNotificationSetting";
        public const string WhatsAppConfigSetting = "WhatsAppConfigSetting";

    public const string EmailSignup = "Signup";
        public const string EmailAddUser = "AddUser";
        public const string SubscriptionShareLimitExceed = "You have exceeded the sharing limit of this plan.";
        public const string CollectionForTody = "Collection For Today";

        public const string NotificationSent = "Notification Sent";
        public const string PushNotificationSetting = "PushNotification";
        public const string PodcastNotificationSetting = "PodcastNotification";
        public const string DailyRemindarNotificationSetting = "DailyRemindarNotification";

        #region Notification Type
        public const string PodcastLive = "Podcast Live";
        public const string PodcastEnd = "Podcast End";
        public const string AddNewBook = "New Book";
        public const string AddNewPodcast = "New podcast";
        public const string PublishPodcastChapter = "Episode Publish";
        public const string SchedulePodcastChapter = "New episode scheduled";
        public const string UpdateSchedulePodcastChapter = "Scheduled episode Updated";
        public const string AddCategory = "Add new category";
        public const string SubscriptionExpired = "Subscription Expired";
        public const string SubscriptionExpiring = "Subscription Expiring";
        #endregion

        #region Whatsapp Notification Type
        public const string WPodcastLive = "Podcast Live";
        public const string WPodcastEnd = "Podcast End";
        public const string WAddNewBook = "New Book";
        public const string WAddNewPodcast = "New podcast";
        public const string WPublishPodcastChapter = "Episode Publish";
        public const string WSchedulePodcastChapter = "New episode scheduled";
        public const string WUpdateSchedulePodcastChapter = "Scheduled episode Updated";
        public const string WAddCategory = "Add new category";
        public const string WSubscriptionExpired = "Subscription Expired";
        public const string WSubscriptionExpiring = "Subscription Expiring";
        public const string WPhoneNoVerification = "Mobile No. Verification";

        #endregion


        public const string SuperAdmin = "SuperAdmin";
        public const string AppUser = "AppUser";
        public const bool True = true;
        public const bool False = false;
        public const string AppUserProfileImages = "AppUserProfileImages";
        public const string ClientProfileImages = "ClientProfileImages";
        public const string OutletProfileImages = "OutletProfileImages";
        public const string PropertyImages = "PropertyImages";
        public const string QrCodeImages = "QrCodeImages";
        public const string otpSentOnEmail = "Please enter the OTP that has been successfully sent to your registered email.";
    }
}
