/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     public int val;
 *     public ListNode next;
 *     public ListNode(int val=0, ListNode next=null) {
 *         this.val = val;
 *         this.next = next;
 *     }
 * }
 */
public class Solution {
	public ListNode SwapPairs(ListNode head) {
		ListNode newHead = head;

		ListNode prev = null;
		ListNode cur = head;
		while (true) {
			ListNode left = cur;
			ListNode right = cur?.next;

			if (left == null || right == null) {
				break;
			} else {
				left.next = right.next;
				right.next = left;

				if (prev == null) {
					newHead = right;
				} else {
					prev.next = right;
				}

				prev = left;
				cur = left.next;
			}
		}

		return newHead;
	}
}
