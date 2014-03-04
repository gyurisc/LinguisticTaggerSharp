using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Linquistics
{
	public partial class LinquisticsViewController : UIViewController
	{
		private UISegmentedControl segmentedControl; 
		private UITextView textView;
		private NSString[] tags = new NSString[] {NSLinguisticTag.Noun, NSLinguisticTag.Verb, NSLinguisticTag.Adjective}; 

		public LinquisticsViewController () : base ("LinquisticsViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void LoadView ()
		{
			base.LoadView ();

			loadTextView ();
			loadSegmentedControl ();
			loadLayoutConstraints ();
		}

		public void loadTextView ()
		{
			this.textView = new UITextView (RectangleF.Empty);
			this.textView.Editable = false;
			this.View.AddSubview (this.textView);
		}

		public void loadSegmentedControl()
		{
			string[] items = new string[] {"Nouns", "Verbs", "Adjectives"};
			this.segmentedControl = new UISegmentedControl(items);

			this.segmentedControl.ValueChanged += (object sender, EventArgs e) => {
				NSString tag = tags[segmentedControl.SelectedSegment];
				highlightLinguisticTag(tag);
			};

			this.View.AddSubview(this.segmentedControl);
		}

		void loadLayoutConstraints ()
		{
			textView.TranslatesAutoresizingMaskIntoConstraints = false;
			segmentedControl.TranslatesAutoresizingMaskIntoConstraints = false;
		

			NSDictionary bindings1 = NSDictionary.FromObjectsAndKeys(
				new NSObject[] { segmentedControl, textView}, 
				new NSString[] {new NSString("_segmentedControl"), new NSString("_textView")});

			NSLayoutConstraint[] c1 = NSLayoutConstraint.FromVisualFormat (
				                                 "V:|-[_segmentedControl]-[_textView]-|", 
				(NSLayoutFormatOptions)0, null, bindings1);

			View.AddConstraints(c1);

			NSDictionary bindings2 = NSDictionary.FromObjectsAndKeys(
				new NSObject[] { textView}, 
				new NSString[] {new NSString("_textView")});

			NSLayoutConstraint[] c2 = NSLayoutConstraint.FromVisualFormat (
				                          "H:|-[_textView]-|", 
				                          (NSLayoutFormatOptions)0, null, bindings2);

			View.AddConstraints(c2);

			NSDictionary bindings3 = NSDictionary.FromObjectsAndKeys(
				new NSObject[] { segmentedControl}, 
				new NSString[] {new NSString("_segmentedControl")});

			NSLayoutConstraint[] c3 = NSLayoutConstraint.FromVisualFormat (
				                          "H:|-[_segmentedControl]-|", 
				                          (NSLayoutFormatOptions)0, null, bindings3);

			View.AddConstraints(c3);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			segmentedControl.SelectedSegment = 0;
			highlightLinguisticTag (NSLinguisticTag.Noun);
		}

		public void highlightLinguisticTag(NSString tag)
		{
			textView.AttributedText = AttributedStringHighlightedForTag (tag);
		}

		public NSAttributedString AttributedStringHighlightedForTag(NSString tag)
		{
			NSString s = new NSString ("Solemnly he came forward and mounted the round gunrest. He faced about and blessed gravely thrice the tower, the surrounding land and the awaking mountains. Then, catching sight of Stephen Dedalus, he bent towards him and made rapid crosses in the air, gurgling in his throat and shaking his head. Stephen Dedalus, displeased and sleepy, leaned his arms on the top of the staircase and looked coldly at the shaking gurgling face that blessed him, equine in its length, and at the light untonsured hair, grained and hued like pale oak.");
			NSRange stringRange = new NSRange (0, s.Length);

			NSMutableAttributedString text = new NSMutableAttributedString (s);
			text.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName("AvenirNext-Regular", 17), stringRange);
			text.AddAttribute (UIStringAttributeKey.ForegroundColor, UIColor.LightGray, stringRange);
		
			NSLinguisticTagger tagger = new NSLinguisticTagger (new NSString[] {NSLinguisticTag.SchemeLexicalClass}, (NSLinguisticTaggerOptions)0);
			tagger.AnalysisString = s; 

			tagger.EnumerateTagsInRange(stringRange, NSLinguisticTag.SchemeLexicalClass, (NSLinguisticTaggerOptions)0, delegate(NSString tokenTag, NSRange tokenRange, NSRange sentenceRange, ref bool stop) {
				if(tokenTag.Equals(tag))
				{
					text.AddAttribute(UIStringAttributeKey.ForegroundColor, UIColor.Black, tokenRange);
				}
			});
				
			return text;
		}
	}
}

